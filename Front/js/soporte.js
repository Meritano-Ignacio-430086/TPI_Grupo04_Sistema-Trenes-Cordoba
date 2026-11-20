// Mantenimiento de trenes: listado, alta y cierre
const TIPOS_MANT = [
    "Preventivo",
    "Correctivo",
    "Predictivo",
    "Inspección general",
    "Revisión de frenos",
    "Limpieza y desinfección",
];

const inputBuscar = document.getElementById("buscarMant");
const selectFiltroTipo = document.getElementById("filtroTipoMant");
const tablaMant = document.getElementById("tablaMant");
const contadorMant = document.getElementById("contadorMant");
const mensajeMant = document.getElementById("mensajeMant");
const formNuevoMant = document.getElementById("formNuevoMant");
const formCerrarMant = document.getElementById("formCerrarMant");

let mantenimientos = [];

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

function claseTipo(tipo) {
    const t = (tipo ?? "").toLowerCase();
    if (t === "correctivo") return "text-bg-danger";
    if (t === "completado" || t === "preventivo") return "text-bg-success";
    if (t === "predictivo") return "text-bg-warning";
    return "text-bg-secondary";
}

function actualizarContador(cantidad) {
    const strong = document.createElement("strong");
    strong.textContent = cantidad;
    contadorMant.replaceChildren(strong, ` ${cantidad === 1 ? "mantenimiento encontrado" : "mantenimientos encontrados"}`);
}

function dibujarTabla(lista) {
    tablaMant.replaceChildren();
    for (const m of lista) {
        const tr = document.createElement("tr");

        const badge = document.createElement("span");
        badge.className = `badge ${claseTipo(m.tipomantenimiento)}`;
        badge.textContent = m.tipomantenimiento ?? "-";

        const unidad = document.createElement("a");
        unidad.className = "riel-id-link";
        unidad.href = "#";
        unidad.textContent = m.unidad ?? "-";

        tr.append(
            celda(fechaCorta(m.fecha)),
            celda(badge),
            celda(unidad),
            celda(bloque("riel-cell-main", m.empleado)),
        );
        tablaMant.appendChild(tr);
    }
}

function filtrar() {
    const texto = inputBuscar.value.trim().toLowerCase();
    const tipo = selectFiltroTipo.value;

    const lista = mantenimientos.filter((m) => {
        if (tipo && m.tipomantenimiento !== tipo) return false;
        if (!texto) return true;
        return [m.unidad, m.tipomantenimiento, m.empleado].some((v) => (v ?? "").toLowerCase().includes(texto));
    });

    actualizarContador(lista.length);
    dibujarTabla(lista);
    if (lista.length === 0 && mantenimientos.length > 0) {
        avisar(mensajeMant, "No hay mantenimientos que coincidan con la búsqueda.", "info");
    } else if (mantenimientos.length > 0) {
        avisar(mensajeMant, "", "");
    }
}

async function cargarMantenimientos() {
    try {
        const datos = claves(await apiGet("/api/Mantenimientos"));
        // más recientes primero
        mantenimientos = datos.sort((a, b) => new Date(b.fecha) - new Date(a.fecha));
        if (mantenimientos.length === 0) {
            actualizarContador(0);
            tablaMant.replaceChildren();
            avisar(mensajeMant, "Todavía no hay mantenimientos registrados.", "info");
            return;
        }
        avisar(mensajeMant, "", "");
        filtrar();
    } catch (error) {
        console.error(error);
        avisar(mensajeMant, error.message, "danger");
    }
}

// Se cargan los tipos en el filtro y en el formulario de alta
for (const tipo of TIPOS_MANT) {
    selectFiltroTipo.add(new Option(tipo, tipo));
    document.getElementById("nuevoMantTipo").add(new Option(tipo, tipo));
}

document.getElementById("btnBuscarMant").addEventListener("click", filtrar);
inputBuscar.addEventListener("input", filtrar);
inputBuscar.addEventListener("keydown", (e) => {
    if (e.key === "Enter") filtrar();
});
selectFiltroTipo.addEventListener("change", filtrar);
document.getElementById("btnRecargarMant").addEventListener("click", cargarMantenimientos);

// Nuevo mantenimiento: POST /api/Mantenimientos/Mantenimientos/Create
formNuevoMant.addEventListener("submit", async (e) => {
    e.preventDefault();
    const boton = document.getElementById("btnCrearMant");
    const payload = {
        tipoMantenimiento: document.getElementById("nuevoMantTipo").value,
        empleadoDNI: parseInt(document.getElementById("nuevoMantDni").value, 10),
        unidadTren: document.getElementById("nuevoMantUnidad").value.trim(),
    };

    boton.disabled = true;
    try {
        await apiPost("/api/Mantenimientos/Mantenimientos/Create", payload);
        formNuevoMant.reset();
        bootstrap.Modal.getInstance(document.getElementById("modalMant"))?.hide();
        await cargarMantenimientos();
        avisar(mensajeMant, "Mantenimiento registrado correctamente.", "success");
    } catch (error) {
        console.error(error);
        bootstrap.Modal.getInstance(document.getElementById("modalMant"))?.hide();
        avisar(mensajeMant, error.message, "danger");
    } finally {
        boton.disabled = false;
    }
});

// Finalizar mantenimiento: POST /api/Mantenimientos/Mantenimientos/Close?dniResponsable=..&unidad=..
formCerrarMant.addEventListener("submit", async (e) => {
    e.preventDefault();
    const boton = document.getElementById("btnCerrarMant");
    const dni = parseInt(document.getElementById("cerrarMantDni").value, 10);
    const unidad = document.getElementById("cerrarMantUnidad").value.trim();

    boton.disabled = true;
    try {
        await apiPost(
            `/api/Mantenimientos/Mantenimientos/Close?dniResponsable=${dni}&unidad=${encodeURIComponent(unidad)}`
        );
        formCerrarMant.reset();
        bootstrap.Modal.getInstance(document.getElementById("modalCerrarMant"))?.hide();
        await cargarMantenimientos();
        avisar(mensajeMant, "Mantenimiento finalizado correctamente.", "success");
    } catch (error) {
        console.error(error);
        bootstrap.Modal.getInstance(document.getElementById("modalCerrarMant"))?.hide();
        avisar(mensajeMant, error.message, "danger");
    } finally {
        boton.disabled = false;
    }
});

cargarMantenimientos();
