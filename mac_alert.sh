
#!/bin/bash


TOPIC="macosEternal"


FOTO_PFAD="/tmp/mac_cam.jpg"

sleep 2

/opt/homebrew/bin/imagesnap -w 1.5 "$FOTO_PFAD" > /dev/null 2>&1

ZEIT=$(date "+%d.%m.%Y um %H:%M:%S")
GERAET=$(hostname)

IP_LOCAL=$(ipconfig getifaddr en0)
if [ -z "$IP_LOCAL" ]; then IP_LOCAL="Nicht verbunden"; fi
IP_PUBLIC=$(curl -s https://ifconfig.me)
if [ -z "$IP_PUBLIC" ]; then IP_PUBLIC="Offline"; fi

BATTERIE_INFO=$(pmset -g batt)
BATTERIE_PROZENT=$(echo "$BATTERIE_INFO" | grep -Eo "\d+%" | head -1)
BERICHT="Zeitpunkt: $ZEIT
Lokale IP: $IP_LOCAL
Oeffentliche IP: $IP_PUBLIC
Batterie: $BATTERIE_PROZENT"

# 5. Bericht UND Foto an ntfy senden
if [ -f "$FOTO_PFAD" ]; then
  # Senden mit Foto als Anhang
  curl \
    -T "$FOTO_PFAD" \
    -H "Title: Mac geoeffnet und entsperrt ($GERAET)" \
    -H "Message: $BERICHT" \
    -H "Tags: camera,warning" \
    -H "Priority: high" \
    "https://ntfy.sh" > /dev/null 2>&1
    
  # Temporäres Foto aus Datenschutzgründen wieder vom Mac löschen
  rm "$FOTO_PFAD"
else
  # Nur Text senden, falls die Kamera blockiert war
  curl \
    -H "Title: Mac geoeffnet (Kamera blockiert)" \
    -H "Message: $BERICHT" \
    -H "Tags: computer,lock" \
    -H "Priority: high" \
    -d "$BERICHT" \
    "https://ntfy.sh" > /dev/null 2>&1
fi







