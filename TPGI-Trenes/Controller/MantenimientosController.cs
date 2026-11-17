using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPGI_Trenes.DTO;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes.Controller
{
    [Route("api/Mantenimientos")]
    [ApiController]
    public class MantenimientosController : ControllerBase
    {
        private readonly IMantenimientosServices _service;
        public MantenimientosController(IMantenimientosServices services)
        {
            this._service = services;
        }
        [HttpPost]
        [Route("Mantenimientos/Create")]
        public async Task<ActionResult<List<Mantenimientos>>> CreateMantenimiento([FromBody] Mantenimientos_CreateDTO mant)
        {
            var ok = await _service.AddMantenimiento(mant);
            return Ok(ok);
        }
        [HttpGet]
        public async Task<ActionResult<List<Mantenimientos_ResponseDTO>>> GetAll()
        {
            var lista = await _service.GetAll();
            if (lista.Count == 0) return NotFound();
            return lista;
        }


    }
}
/*
 Para acceder a las tablas en sql:
select tm.Descripcion [Tipo mantenimiento],
e.Nombre + space(1) + e.Apellido [Nombre y apellido empleado], 
tre.Nro_unidad [Tren (Unidad)],
m.Fecha [Fecha y hora mantenimiento]
from Mantenimientos m
join Tipos_Mantenimientos tm on tm.Id_Tipo_Mantenimiento=m.Id_Tipo_Mantenimiento
join empleados e on e.Id_Empleado = m.Id_Empleado
join Trenes tre on tre.Id_Tren = m.Id_Tren

select * from Mantenimientos
select * from Facturas
select * from Detalles_Facturas

select * from Empleados

select * from 
select * from Mantenimientos
select * from Tipos_Mantenimientos
select * from Detalles_Facturas
select * from Tickets

select * from Trenes
select * from Estados

Alta de mantenimiento: 
Se debería solicitar los datos del encargado
La unidad que se quiere arreglar

Baja de manteniemiento:
Actualización del mantenimiento:


Un alta mantenimientos debería cambiar el estado del tren, solicitar el tipo de mantenimiento y la unidad
Una baja de mantenimiento debería ser como un update, debería colocar el estado del tren resuelto (Fuera de Servicio, En reserva)
El costo debería ser calculado a través de los materiales y presupuesto con el area de mantenimiento

*/