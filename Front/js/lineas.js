// ===== Pestañas (Líneas / Choferes) =====
const pestanias = document.querySelectorAll(".pestania");

for (const boton of pestanias) {
    boton.addEventListener("click", () => {
        for (const otro of pestanias) {
            otro.classList.remove("activa");
            document.getElementById(otro.dataset.panel).classList.add("d-none");
        }
        boton.classList.add("activa");
        document.getElementById(boton.dataset.panel).classList.remove("d-none");
    });
}

// Evita que un texto de la base rompa el HTML
function escapar(texto) {
    const div = document.createElement("div");
    div.textContent = texto ?? "";
    return div.innerHTML;
}

// Elige el color del badge según el estado
function claseEstado(estado) {
    const texto = (estado ?? "").toLowerCase();
    if (texto.includes("mantenimiento") || texto.includes("repar")) return "text-bg-warning";
    if (texto.includes("fuera") || texto.includes("baja") || texto.includes("inactiv")) return "text-bg-danger";
    if (texto.includes("activ") || texto.includes("operativ") || texto.includes("servicio")) return "text-bg-success";
    return "text-bg-secondary";
}

// Muestra el spinner en el botón mientras busca
function cargando(boton, activo) {
    boton.disabled = activo;
    boton.innerHTML = activo
        ? '<span class="spinner-border spinner-border-sm"></span> Buscando...'
        : '<i class="bi bi-search"></i> Buscar';
}

// ===== Líneas =====
const form = document.getElementById("formBuscar");
const inputLinea = document.getElementById("nroLinea");
const btnBuscar = document.getElementById("btnBuscar");
const mensaje = document.getElementById("mensaje");
const vacioLineas = document.getElementById("vacioLineas");
const resultados = document.getElementById("resultados");
const tarjetas = document.getElementById("tarjetasTrenes");

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const nroLinea = parseInt(inputLinea.value, 10);
    if (!Number.isInteger(nroLinea) || nroLinea < 1) {
        mostrarMensaje("Ingresá un número de línea válido.", "warning");
        return;
    }

    cargando(btnBuscar, true);
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
        cargando(btnBuscar, false);
    }
});

// Nro de Linea
async function buscarLinea(nroLinea) {
    const datos = await apiGet(`/api/Lineas/${nroLinea}`);
    return datos ?? [];
}

function mostrarResultados(trenes) {
    const linea = trenes[0];
    document.getElementById("cantTrenes").textContent = trenes.length;
    document.getElementById("infoLinea").textContent =
        `Línea ${linea.numeroLinea} · ${linea.cantidadEstacionesEnRecorrido} estaciones`;

    tarjetas.innerHTML = trenes.map(tarjetaTren).join("");
    resultados.classList.remove("d-none");
}

function tarjetaTren(tren) {
    const estado = tren.estadoOperativoTren ?? "Sin estado";
    const claseBadge = claseEstado(estado);
    // franja de arriba: roja si no está operativo, celeste si sí
    const colorFranja = claseBadge === "text-bg-success" ? "franja-ok" : "franja-alerta";
    const nroLinea = String(tren.numeroLinea).padStart(2, "0");

    return `
        <div class="col-md-6">
            <article class="card tarjeta-tren h-100">
                <div class="franja-tren ${colorFranja}"></div>
                <div class="p-4">
                    <div class="d-flex justify-content-between align-items-start gap-3">
                        <div>
                            <span class="eyebrow mb-1">Línea ${nroLinea}</span>
                            <h3 class="tren-unidad">Unidad ${escapar(tren.unidadTren ?? "-")}</h3>
                            <small class="tren-modelo">${escapar(tren.modeloTren ?? "-")}</small>
                        </div>
                        <span class="badge ${claseBadge}">${escapar(estado)}</span>
                    </div>

                    <p class="tren-recorrido">${escapar(tren.descripcionRecorrido ?? "Sin descripción")}</p>

                    <div class="tren-datos">
                        <div class="row g-3">
                            <div class="col-6 col-sm-3">
                                <small>Estaciones</small>
                                <strong>${tren.cantidadEstacionesEnRecorrido}</strong>
                            </div>
                            <div class="col-6 col-sm-3">
                                <small>Vagones</small>
                                <strong>${tren.cantidadVagonesAsignados}</strong>
                            </div>
                            <div class="col-6 col-sm-3">
                                <small>Capacidad</small>
                                <strong>${tren.capacidadTotalVagones} pax</strong>
                            </div>
                            <div class="col-6 col-sm-3">
                                <small>Mantenimientos</small>
                                <strong class="text-primary">${tren.totalMantenimientosHistoricos}</strong>
                            </div>
                        </div>
                    </div>

                    <p class="tren-pie">
                        Último mantenimiento: <strong>${formatearFecha(tren.ultimaFechaMantenimiento)}</strong>
                    </p>
                </div>
            </article>
        </div>`;
}

function formatearFecha(fecha) {
    if (!fecha) return "Sin mantenimientos";
    return new Date(fecha).toLocaleDateString("es-AR", {
        day: "2-digit",
        month: "short",
        year: "numeric",
    });
}

function mostrarMensaje(texto, tipo) {
    mensaje.textContent = texto;
    mensaje.className = `alert alert-${tipo} mt-4`;
    vacioLineas.classList.add("d-none");
}

function ocultarTodo() {
    mensaje.className = "alert d-none mt-4";
    vacioLineas.classList.add("d-none");
    resultados.classList.add("d-none");
}

// ===== Choferes =====
const formChoferes = document.getElementById("formChoferes");
const btnChoferes = formChoferes.querySelector("button");
const inputChofer = document.getElementById("nombreChofer");
const selectCarga = document.getElementById("cargaChofer");
const mensajeChoferes = document.getElementById("mensajeChoferes");
const resultadosChoferes = document.getElementById("resultadosChoferes");
const tablaChoferes = document.getElementById("tablaChoferes");

formChoferes.addEventListener("submit", async (e) => {
    e.preventDefault();
    avisar(mensajeChoferes, "", "");
    resultadosChoferes.classList.add("d-none");
    cargando(btnChoferes, true);

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

        document.getElementById("cantChoferes").textContent = choferes.length;
        tablaChoferes.innerHTML = choferes.map(filaChofer).join("");
        resultadosChoferes.classList.remove("d-none");
    } catch (error) {
        console.error(error);
        avisar(mensajeChoferes, error.message, "danger");
    } finally {
        cargando(btnChoferes, false);
    }
});

// "Lucía Fernández" -> "LF"
function iniciales(nombre) {
    return (nombre ?? "")
        .split(" ")
        .filter((parte) => parte)
        .slice(0, 2)
        .map((parte) => parte[0].toUpperCase())
        .join("");
}

function filaChofer(c) {
    const carga = c.estado_carga_horaria ?? "-";
    const claseCarga = carga === "Por encima del promedio" ? "text-bg-warning" : "text-bg-success";

    return `
        <tr>
            <td>
                <div class="d-flex align-items-center gap-3">
                    <span class="avatar-chico">${escapar(iniciales(c.chofer))}</span>
                    <div>
                        <strong class="d-block">${escapar(c.chofer)}</strong>
                        <small class="text-muted">DNI ${escapar(c.dni)}</small>
                    </div>
                </div>
            </td>
            <td>${escapar(c.barrio)}</td>
            <td>${escapar(c.telefono)}</td>
            <td class="text-end">${c.viajes_realizados}</td>
            <td class="text-end">${c.trenes_conducidos}</td>
            <td class="text-end">${c.minutos_conducidos}</td>
            <td><span class="badge ${claseCarga}">${escapar(carga)}</span></td>
        </tr>`;
}