var myModal = document.getElementById('exampleModal')
var myInput = document.getElementById('myInput')

myModal.addEventListener('shown.bs.modal', function () {
  myInput.focus()
})

// Historial de compra por DNI
const inputDni = document.getElementById("dniTicket");
const tablaHistorial = document.getElementById("tablaHistorial");
const mensajeHistorial = document.getElementById("mensajeHistorial");

document.getElementById("btnHistorial").addEventListener("click", async () => {
    const dni = parseInt(inputDni.value, 10);
    tablaHistorial.replaceChildren();
    if (!Number.isInteger(dni) || dni < 1) {
        avisar(mensajeHistorial, "Ingresá un DNI válido.", "warning");
        return;
    }

    try {
        const compras = claves(await apiGet(`/api/Lineas/historial-cliente/${dni}`));
        if (compras.length === 0) {
            avisar(mensajeHistorial, `No se encontraron compras para el DNI ${dni}.`, "info");
            return;
        }
        avisar(mensajeHistorial, "", "");
        llenarTabla(tablaHistorial, compras, (c) => [
            c.id_ticket, fechaCorta(c.fecha_compra), c.nro_linea, c.recorrido,
            c.estacion, c.tren, c.hora_partida, c.hora_llegada, moneda(c.precio),
        ]);
    } catch (error) {
        console.error(error);
        avisar(mensajeHistorial, error.message, "danger");
    }
});

// Facturas: el selector filtra entre importantes (vista vw_Compras_Importantes)
// y erróneas (facturas con varios pagos que tienen diferencia o forma de pago repetida).
// Si no se elige ninguna, se muestran todas.
const tablaFacturas = document.getElementById("tablaFacturas");
const mensajeFacturas = document.getElementById("mensajeFacturas");
const selectTipoFactura = document.getElementById("tipoFactura");

async function cargarFacturas() {
    const tipo = selectTipoFactura.value;
    tablaFacturas.replaceChildren();
    try {
        const [importantes, erroneas] = await Promise.all([
            tipo === "erroneas" ? [] : apiGet("/api/Lineas/compras-importantes"),
            tipo === "importantes" ? [] : apiGet("/api/Lineas/facturas"),
        ]);

        // Una misma factura puede ser importante y errónea a la vez.
        const porNumero = new Map();
        const agregar = (nro, datos, etiqueta) => {
            const actual = porNumero.get(nro) ?? { nro, tipos: [], observacion: "-" };
            Object.assign(actual, datos);
            actual.tipos.push(etiqueta);
            porNumero.set(nro, actual);
        };

        for (const f of claves(importantes)) {
            agregar(f.nro_factura, {
                proveedor: f.proveedor, fecha: f.fecha, monto: f.total,
            }, "Importante");
        }
        for (const f of claves(erroneas)) {
            const conError = Math.abs(f.diferencia ?? 0) > 0 || /error/i.test(f.observacion ?? "");
            if (!conError) continue;
            agregar(f.nro_factura, {
                proveedor: f.proveedor, fecha: f.fecha, monto: f.montodelafactura,
                observacion: f.observacion,
            }, "Errónea");
        }

        const filas = [...porNumero.values()].sort((a, b) => a.nro - b.nro);
        if (filas.length === 0) {
            avisar(mensajeFacturas, "No hay facturas para mostrar.", "info");
            return;
        }
        avisar(mensajeFacturas, "", "");
        llenarTabla(tablaFacturas, filas, (f) => [
            f.nro, f.proveedor, fechaCorta(f.fecha), moneda(f.monto),
            f.tipos.join(" / "), f.observacion,
        ]);
    } catch (error) {
        console.error(error);
        avisar(mensajeFacturas, error.message, "danger");
    }
}

document.getElementById("btnFacturas").addEventListener("click", cargarFacturas);
selectTipoFactura.addEventListener("change", cargarFacturas);
