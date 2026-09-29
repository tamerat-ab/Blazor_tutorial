# EF Core Relationships Tutorial

## 1. Relationship types in EF Core

EF Core supports three main relationship shapes:

1. One-to-One
2. One-to-Many
3. Many-to-Many

These relationships are built from navigation properties and foreign keys in the model classes.

---

## 2. One-to-One relationship

A row in one table matches exactly one row in another table.

Example:

- One `Employee` has one `EmployeeProfile`
- One `EmployeeProfile` belongs to one `Employee`

Class example:

```csharp
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public EmployeeProfile? Profile { get; set; }
}

public class EmployeeProfile
{
    public int Id { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
```

Fluent API relationship mapping:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>()
        .HasOne(e => e.Profile)
        .WithOne(p => p.Employee)
        .HasForeignKey<EmployeeProfile>(p => p.EmployeeId);
}
```

---

## 3. One-to-Many relationship

One parent row can match many child rows.

Example:

- One `Department` has many `Employees`
- Many `Employees` belong to one `Department`

Class example:

```csharp
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Employee> Employees { get; set; } = new();
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
}
```

Fluent API relationship mapping:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>()
        .HasOne(e => e.Department)
        .WithMany(d => d.Employees)
        .HasForeignKey(e => e.DepartmentId);
}
```

---

## 4. Many-to-Many relationship

Many rows in one table can match many rows in another table.

Example:

- Many `Employees` can work on many `Projects`
- Many `Projects` can have many `Employees`

Class example:

```csharp
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Project> Projects { get; set; } = new();
}

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Employee> Employees { get; set; } = new();
}
```

Fluent API relationship mapping:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>()
        .HasMany(e => e.Projects)
        .WithMany(p => p.Employees)
        .UsingEntity(j => j.ToTable("EmployeeProject"));
}
```

This creates a join table such as `EmployeeProject`.

---

## 5. Navigation properties

Navigation properties connect entities together.

Examples:

```csharp
public Department? Department { get; set; }
public List<Employee> Employees { get; set; } = new();
```

- Reference navigation: points to one object
- Collection navigation: points to many objects

---

## 6. Foreign key property

The child side holds the foreign key:

```csharp
public int DepartmentId { get; set; }
```

This column stores the parent ID in the database.

---

## 7. Data annotations and Fluent API

Data annotations are placed directly on classes and properties:

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Employees")]
public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
```

Fluent API is configured in `DbContext.OnModelCreating()`:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .ToTable("Employees");

        modelBuilder.Entity<Employee>()
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
```

Use data annotations for simple property rules. Use Fluent API for relationships, indexes, constraints, and advanced schema rules.

---

## 8. Headline summary

The database relationship is expressed in classes by:

- a foreign key property on the child
- a navigation property on the parent
- a navigation property on the child

And EF Core routes that mapping to SQL tables and joins for you.
