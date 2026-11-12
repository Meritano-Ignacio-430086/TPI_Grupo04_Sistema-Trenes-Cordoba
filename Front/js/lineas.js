// Direcciones de la API (ver TPGI-Trenes/Properties/launchSettings.json).
// Se prueba primero https y, si no responde, http.
const API_URLS = ["https://localhost:7091", "http://localhost:5282"];

const form = document.getElementById("formBuscar");
const inputLinea = document.getElementById("nroLinea");
const btnBuscar = document.getElementById("btnBuscar");
const mensaje = document.getElementById("mensaje");
const resultados = document.getElementById("resultados");
const tabla = document.getElementById("tablaTrenes");

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const nroLinea = parseInt(inputLinea.value, 10);
    if (!Number.isInteger(nroLinea) || nroLinea < 1) {
        mostrarMensaje("Ingresá un número de línea válido.", "warning");
        return;
    }

    btnBuscar.disabled = true;
    ocultarTodo();

    try {
        const trenes = await buscarLinea(nroLinea);
        if (trenes.length === 0) {
            mostrarMensaje(`No se encontraron trenes para la línea ${nroLinea}.`, "info");
        } else {
            mostrarResultados(trenes);
        }
    } catch (error) {
        console.error(error);
        mostrarMensaje(error.message, "danger");
    } finally {
        btnBuscar.disabled = false;
    }
});

// GET api/Lineas?NroLinea=N -> lista de LineasDTO
async function buscarLinea(nroLinea) {
    for (const base of API_URLS) {
        let respuesta;
        try {
            respuesta = await fetch(`${base}/api/Lineas?NroLinea=${nroLinea}`);
        } catch {
            continue; // esa dirección no responde, se prueba la siguiente
        }

        if (respuesta.status === 204) return [];
        if (!respuesta.ok) {
            throw new Error(`La API devolvió un error (${respuesta.status}).`);
        }
        return await respuesta.json();
    }
    throw new Error("No se pudo conectar con la API. Verificá que el backend esté corriendo.");
}

function mostrarResultados(trenes) {
    const linea = trenes[0];
    document.getElementById("resNroLinea").textContent = linea.numeroLinea;
    document.getElementById("resRecorrido").textContent = linea.descripcionRecorrido ?? "Sin descripción";
    document.getElementById("resEstaciones").textContent = linea.cantidadEstacionesEnRecorrido;
    document.getElementById("resTrenes").textContent = trenes.length;

    tabla.replaceChildren();
    for (const tren of trenes) {
        const fila = document.createElement("tr");
        agregarCelda(fila, tren.unidadTren ?? "-");
        agregarCelda(fila, tren.modeloTren ?? "-");
        fila.appendChild(celdaEstado(tren.estadoOperativoTren));
        agregarCelda(fila, tren.cantidadVagonesAsignados, "text-end");
        agregarCelda(fila, tren.capacidadTotalVagones, "text-end");
        agregarCelda(fila, tren.totalMantenimientosHistoricos, "text-end");
        agregarCelda(fila, formatearFecha(tren.ultimaFechaMantenimiento));
        tabla.appendChild(fila);
    }

    resultados.classList.remove("d-none");
}

function agregarCelda(fila, texto, clase) {
    const celda = document.createElement("td");
    celda.textContent = texto;
    if (clase) celda.className = clase;
    fila.appendChild(celda);
}

function celdaEstado(estado) {
    const celda = document.createElement("td");
    const badge = document.createElement("span");
    const texto = (estado ?? "").toLowerCase();

    let color = "text-bg-secondary";
    if (texto.includes("mantenimiento") || texto.includes("repar")) color = "text-bg-warning";
    else if (texto.includes("fuera") || texto.includes("baja") || texto.includes("inactiv")) color = "text-bg-danger";
    else if (texto.includes("activ") || texto.includes("operativ") || texto.includes("servicio")) color = "text-bg-success";

    badge.className = `badge ${color}`;
    badge.textContent = estado ?? "Sin estado";
    celda.appendChild(badge);
    return celda;
}

function formatearFecha(fecha) {
    if (!fecha) return "Sin mantenimientos";
    return new Date(fecha).toLocaleDateString("es-AR");
}

function mostrarMensaje(texto, tipo) {
    mensaje.textContent = texto;
    mensaje.className = `alert alert-${tipo}`;
}

function ocultarTodo() {
    mensaje.className = "alert d-none";
    resultados.classList.add("d-none");
}
