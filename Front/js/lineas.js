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

// Nro de Linea
async function buscarLinea(nroLinea) {
    const datos = await apiGet(`/api/Lineas?NroLinea=${nroLinea}`);
    return datos ?? [];
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

// -> Choferes
const formChoferes = document.getElementById("formChoferes");
const inputChofer = document.getElementById("nombreChofer");
const selectCarga = document.getElementById("cargaChofer");
const mensajeChoferes = document.getElementById("mensajeChoferes");
const resultadosChoferes = document.getElementById("resultadosChoferes");
const tablaChoferes = document.getElementById("tablaChoferes");

formChoferes.addEventListener("submit", async (e) => {
    e.preventDefault();
    avisar(mensajeChoferes, "", "");
    resultadosChoferes.classList.add("d-none");

    try {
        const nombre = inputChofer.value.trim().toLowerCase();
        const carga = selectCarga.value;
        const choferes = claves(await apiGet("/api/Lineas/choferes")).filter(
            (c) =>
                (c.chofer ?? "").toLowerCase().includes(nombre) &&
                (!carga || c.estado_carga_horaria === carga)
        );

        if (choferes.length === 0) {
            avisar(mensajeChoferes, "No se encontraron choferes.", "info");
            return;
        }

        llenarTabla(tablaChoferes, choferes, (c) => [
            c.chofer, c.dni, c.barrio, c.telefono,
            c.viajes_realizados, c.trenes_conducidos, c.minutos_conducidos,
            c.estado_carga_horaria,
        ]);
        resultadosChoferes.classList.remove("d-none");
    } catch (error) {
        console.error(error);
        avisar(mensajeChoferes, error.message, "danger");
    }
});
