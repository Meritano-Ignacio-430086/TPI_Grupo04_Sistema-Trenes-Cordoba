// Se definen dos urls
const API_URLS = ["https://localhost:7091", "http://localhost:5282"];

// La función devuelve un json, o null si la api responde 204 o 404
async function apiGet(ruta) {
    for (const base of API_URLS) {
        let respuesta;
        try {
            respuesta = await fetch(base + ruta);
        } catch {
            continue; // esa dirección no responde, se prueba la siguiente
        }

        if (respuesta.status === 204 || respuesta.status === 404) return null;
        if (!respuesta.ok) {
            throw new Error(`La API devolvió un error (${respuesta.status}).`);
        }
        return await respuesta.json();
    }
    throw new Error("No se pudo conectar con la API. Verificá que el backend esté corriendo.");
}

// POST con cuerpo JSON; devuelve el JSON de la respuesta (o true si no trae cuerpo).
// Si la API responde con error, lanza un Error con .status y el mensaje que haya enviado.
async function apiPost(ruta, cuerpo) {
    for (const base of API_URLS) {
        let respuesta;
        try {
            respuesta = await fetch(base + ruta, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(cuerpo),
            });
        } catch {
            continue; // esa dirección no responde, se prueba la siguiente
        }

        if (respuesta.status === 204) throw new Error("La API devolvió 204 NoContent.");
        if (!respuesta.ok) {
            let detalle = "";
            try {
                const datos = await respuesta.json();
                detalle = typeof datos === "string" ? datos : datos.mensaje ?? datos.message ?? "";
            } catch { /* la respuesta no trae JSON */ }
            const error = new Error(detalle || `La API devolvió un error (${respuesta.status}).`);
            error.status = respuesta.status;
            throw error;
        }
        try {
            return await respuesta.json();
        } catch {
            return true;
        }
    }
    throw new Error("No se pudo conectar con la API. Verificá que el backend esté corriendo.");
}

function claves(lista) {
    return (lista ?? []).map((item) =>
        Object.fromEntries(Object.entries(item).map(([k, v]) => [k.toLowerCase(), v]))
    );
}

function moneda(valor) {
    return Number(valor ?? 0).toLocaleString("es-AR", { style: "currency", currency: "ARS" });
}

function fechaCorta(fecha) {
    return fecha ? new Date(fecha).toLocaleDateString("es-AR") : "-";
}

// Arma el tbody a partir de una lista y una función que devuelve las celdas de cada fila.
function llenarTabla(tbody, filas, celdas) {
    tbody.replaceChildren();
    for (const dato of filas) {
        const tr = document.createElement("tr");
        for (const texto of celdas(dato)) {
            const td = document.createElement("td");
            td.textContent = texto ?? "-";
            tr.appendChild(td);
        }
        tbody.appendChild(tr);
    }
}

function avisar(elemento, texto, tipo) {
    elemento.textContent = texto;
    elemento.className = texto ? `alert alert-${tipo}` : "alert d-none";
}
