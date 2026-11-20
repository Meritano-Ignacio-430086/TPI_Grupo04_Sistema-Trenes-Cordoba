// Historial de compras por documento
const inputDni = document.getElementById("dniTicket");
const tablaHistorial = document.getElementById("tablaHistorial");
const mensajeHistorial = document.getElementById("mensajeHistorial");
const contadorTickets = document.getElementById("contadorTickets");
const btnExportar = document.getElementById("btnExportarTickets");
const formNuevoTicket = document.getElementById("formNuevoTicket");

let ticketsActuales = [];

function actualizarContador(cantidad) {
    const strong = document.createElement("strong");
    strong.textContent = cantidad;
    contadorTickets.replaceChildren(strong, ` ${cantidad === 1 ? "ticket encontrado" : "tickets encontrados"}`);
}

function celda(...hijos) {
    const td = document.createElement("td");
    td.append(...hijos);
    return td;
}

function bloque(clase, texto) {
    const span = document.createElement("span");
    span.className = clase;
    span.textContent = texto ?? "-";
    return span;
}

function fechaHora(fecha) {
    if (!fecha) return "-";
    const f = new Date(fecha);
    const dia = f.toLocaleDateString("es-AR", { day: "2-digit", month: "short", year: "numeric" }).replace(/\./g, "").replace(/ de /g, " ").replace(/\b[a-záéíóú]/g, (m, i) => (i > 2 ? m.toUpperCase() : m));
    const hora = f.toLocaleTimeString("es-AR", { hour: "2-digit", minute: "2-digit", hour12: false });
    return `${dia} · ${hora}`;
}

function hora(valor) {
    return valor ? String(valor).slice(0, 5) : "-";
}

// /api/Tickets/Clientes devuelve tickets "crudos" (idTicket, fecha, idCliente, idEmpleado).
// Se pasan al mismo formato que usa la tabla; lo que ese endpoint no trae queda vacío.
function normalizarTicket(t) {
    return {
        id_ticket: t.idticket,
        id_cliente: t.idcliente,
        id_empleado: t.idempleado,
        fecha_compra: t.fecha,
    };
}

function dibujarTabla(compras, dni) {
    const hayDni = Number.isInteger(dni) && dni > 0;
    tablaHistorial.replaceChildren();
    for (const c of compras) {
        const tr = document.createElement("tr");

        const id = document.createElement("a");
        id.className = "riel-id-link";
        id.href = "#";
        id.textContent = `#${c.id_ticket}`;

        const tren = document.createElement("a");
        tren.className = "riel-id-link";
        tren.href = "#";
        tren.textContent = c.tren ?? "-";

        tr.append(
            celda(id),
            celda(
                bloque("riel-cell-main", c.nombre_cliente ?? (c.id_cliente ? `Cliente #${c.id_cliente}` : "Cliente")),
                bloque("riel-cell-sub", hayDni ? `${c.tipo_doc ?? "DNI"} ${dni}` : (c.id_empleado ? `Empleado #${c.id_empleado}` : "-")),
            ),
            celda(fechaHora(c.fecha_compra)),
            celda((c.id_det_ticket ?? c.id_detalle) != null ? `#${c.id_det_ticket ?? c.id_detalle}` : "-"),
            celda(
                bloque("riel-cell-main", c.nro_linea != null ? `Línea ${c.nro_linea}` : "-"),
                bloque("riel-cell-sub", c.nro_linea != null ? c.recorrido : ""),
            ),
            celda(c.estacion ?? "-"),
            celda(tren),
            celda(c.hora_partida || c.hora_llegada ? `${hora(c.hora_partida)} → ${hora(c.hora_llegada)}` : "-"),
            celda(bloque("riel-price", c.precio != null ? moneda(c.precio) : "-")),
        );
        tr.lastElementChild.classList.add("text-end");
        tablaHistorial.appendChild(tr);
    }
}

async function consultarHistorial() {
    const dni = parseInt(inputDni.value, 10);
    tablaHistorial.replaceChildren();
    ticketsActuales = [];
    actualizarContador(0);

    // Con un DNI válido se busca el historial de ese cliente; sin DNI, se listan todos los tickets
    const hayDni = Number.isInteger(dni) && dni > 0;

    try {
        let compras;
        if (hayDni) {
            compras = claves(await apiGet(`/api/Tickets/Clientes/${dni}`));
        } else {
            compras = claves(await apiGet("/api/Tickets/Clientes"))
                //.map(normalizarTicket)
                //.sort((a, b) => b.id_ticket - a.id_ticket); // los más nuevos primero
        }
        if (compras.length === 0) {
            avisar(
                mensajeHistorial,
                hayDni ? `No se encontraron compras para el documento ${dni}.` : "Todavía no hay tickets cargados.",
                "info"
            );
            return;
        }
        avisar(mensajeHistorial, "", "");
        ticketsActuales = compras;
        actualizarContador(compras.length);
        dibujarTabla(compras, dni);
    } catch (error) {
        console.error(error);
        avisar(mensajeHistorial, error.message, "danger");
    }
}

document.getElementById("btnHistorial").addEventListener("click", consultarHistorial);
inputDni.addEventListener("keydown", (e) => {
    if (e.key === "Enter") consultarHistorial();
});

// Exportar el listado actual a CSV
btnExportar.addEventListener("click", () => {
    if (ticketsActuales.length === 0) {
        avisar(mensajeHistorial, "No hay tickets para exportar.", "info");
        return;
    }
    const columnas = ["id_ticket", "fecha_compra", "nro_linea", "recorrido", "estacion", "tren", "hora_partida", "hora_llegada", "precio"];
    const filas = ticketsActuales.map((c) =>
        columnas.map((k) => `"${String(c[k] ?? "").replace(/"/g, '""')}"`).join(",")
    );
    const blob = new Blob([[columnas.join(","), ...filas].join("\n")], { type: "text/csv;charset=utf-8" });
    const enlace = document.createElement("a");
    enlace.href = URL.createObjectURL(blob);
    enlace.download = "tickets.csv";
    enlace.click();
    URL.revokeObjectURL(enlace.href);
});

// Nuevo ticket: POST /api/Tickets/Ticket/Create
const modalTicketEl = document.getElementById("modalTicket");
const campo = (id) => document.getElementById(id);
const btnCrearTicket = document.getElementById("btnCrearTicket");

const selEstacion = campo("nuevoTicketEstacion");
const selLinea = campo("nuevoTicketLinea");
const selHora = campo("nuevoTicketHoraPartida");
const mensajeNuevoTicket = campo("mensajeNuevoTicket");

// Rellena un select con opciones; si no hay nada que mostrar, lo deja deshabilitado
function llenarSelect(select, valores, textoInicial) {
    select.replaceChildren(new Option(textoInicial, ""));
    for (const v of valores) select.add(new Option(v, v));
    select.disabled = valores.length === 0;
}

// Estación -> Línea -> Hora: cada select solo muestra lo que existe para lo elegido antes
function cargarEstaciones() {
    llenarSelect(selEstacion, Object.keys(SALIDAS).sort((a, b) => a.localeCompare(b, "es")), "Elegí una estación");
    selLinea.replaceChildren(new Option("Primero elegí la estación", ""));
    selLinea.disabled = true;
    selHora.replaceChildren(new Option("Primero elegí la línea", ""));
    selHora.disabled = true;
}

selEstacion.addEventListener("change", () => {
    const lineas = Object.keys(SALIDAS[selEstacion.value] ?? {});
    llenarSelect(selLinea, lineas, lineas.length ? "Elegí una línea" : "Primero elegí la estación");
    selHora.replaceChildren(new Option("Primero elegí la línea", ""));
    selHora.disabled = true;
});

selLinea.addEventListener("change", () => {
    const horas = SALIDAS[selEstacion.value]?.[selLinea.value] ?? [];
    llenarSelect(selHora, horas, horas.length ? "Elegí un horario" : "Primero elegí la línea");
    // si hay un solo horario, queda elegido
    if (horas.length === 1) selHora.value = horas[0];
});

// Al limpiar el formulario o cerrar el modal se vuelve al estado inicial
formNuevoTicket.addEventListener("reset", () => setTimeout(cargarEstaciones));
modalTicketEl.addEventListener("hidden.bs.modal", () => {
    formNuevoTicket.reset();
    avisar(mensajeNuevoTicket, "", "");
});
cargarEstaciones();

formNuevoTicket.addEventListener("submit", async (e) => {
    e.preventDefault();
    avisar(mensajeNuevoTicket, "", "");

    const formasPago = [...campo("nuevoTicketFormasPago").querySelectorAll("input:checked")].map((c) => c.value);
    if (formasPago.length === 0) {
        avisar(mensajeNuevoTicket, "Elegí al menos una forma de pago.", "warning");
        return;
    }

    const dniCliente = parseInt(campo("nuevoTicketDniCliente").value, 10);
    const payload = {
        dniEmpleado: parseInt(campo("nuevoTicketDniEmpleado").value, 10),
        dniCliente,
        detalleTickets: [
            {
                nroLinea: parseInt(campo("nuevoTicketLinea").value, 10),
                estacion: campo("nuevoTicketEstacion").value.trim(),
                horaPartida: campo("nuevoTicketHoraPartida").value.trim(),
                tipoVagon: campo("nuevoTicketTipoVagon").value.trim(),
            },
        ],
        formasPago,
    };

    btnCrearTicket.disabled = true;
    try {
        await apiPost("/api/Tickets/Ticket/Create", payload);
        formNuevoTicket.reset();
        bootstrap.Modal.getInstance(modalTicketEl)?.hide();
        // Muestra el historial del cliente al que se le emitió el ticket
        inputDni.value = dniCliente;
        await consultarHistorial();
        avisar(mensajeHistorial, "Ticket emitido correctamente.", "success");
    } catch (error) {
        console.error(error);
        // el modal sigue abierto: el error se muestra adentro para que se vea
        avisar(mensajeNuevoTicket, error.message, "danger");
    } finally {
        btnCrearTicket.disabled = false;
    }
});
