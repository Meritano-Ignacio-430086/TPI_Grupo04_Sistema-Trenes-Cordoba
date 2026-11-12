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

// Facturas con varios pagos, esto consulta la tabla de facturas
// y consulta también la tabla de detalles de cada factura.
const tablaFacturas = document.getElementById("tablaFacturas");
const mensajeFacturas = document.getElementById("mensajeFacturas");

document.getElementById("btnFacturas").addEventListener("click", async () => {
    tablaFacturas.replaceChildren();
    try {
        const facturas = claves(await apiGet("/api/Lineas/facturas"));
        if (facturas.length === 0) {
            avisar(mensajeFacturas, "No hay facturas para mostrar.", "info");
            return;
        }
        avisar(mensajeFacturas, "", "");
        llenarTabla(tablaFacturas, facturas, (f) => [
            f.nro_factura, f.proveedor, fechaCorta(f.fecha), moneda(f.montodelafactura),
            moneda(f.totaldeldetalle), moneda(f.diferencia), f.observacion,
        ]);
    } catch (error) {
        console.error(error);
        avisar(mensajeFacturas, error.message, "danger");
    }
});
