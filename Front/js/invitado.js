// Ingreso como invitado: sesión sin cuenta, solo para consultar
document.getElementById("btnInvitado").addEventListener("click", (e) => {
    e.preventDefault();
    iniciarInvitado();
    location.href = "lineas.html";
});
