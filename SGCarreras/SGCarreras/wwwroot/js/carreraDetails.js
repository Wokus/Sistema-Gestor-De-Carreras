// Singleton de conexión global
if (!window.globalSignalRConnection) {
    window.globalSignalRConnection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7140/carreraHub") // puerto API
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Suscripciones activas
    window.carrerasActivas = new Set();

    window.globalSignalRConnection.on("CorredorAvanzo", data => {
        // Filtrar solo eventos de la carrera actual
        if (window.carreraId !== data.id) return;

        const tbody = document.getElementById("tabla-corredores");
        let row = document.querySelector(`#row-${data.corredor}`);
        if (!row) {
            row = document.createElement("tr");
            row.id = `row-${data.corredor}`;
            row.innerHTML = `<td>${data.corredor}</td><td></td><td></td>`;
            tbody.appendChild(row);
        }
        row.children[1].textContent = data.punto;
        row.children[2].textContent = new Date(data.fecha).toLocaleTimeString();
    });

    window.globalSignalRConnection.start()
        .then(() => console.log("✅ Conectado a SignalR global"))
        .catch(err => console.error("❌ Error SignalR:", err));
}

// Cuando se carga la vista Details
document.addEventListener("DOMContentLoaded", async () => {
    const id = window.carreraId;
    if (!window.carrerasActivas.has(id)) {
        await window.globalSignalRConnection.invoke("UnirseACarrera", id);
        window.carrerasActivas.add(id);
        console.log("📡 Suscrito a carrera", id);
    }
});
