// Sesión del front: guarda el token que devuelve POST /api/Auth/login.
// "Recordarme" lo deja en localStorage; si no, solo dura mientras esté abierta la pestaña.
const CLAVE_TOKEN = "rielcba_token";
const CLAVE_USUARIO = "rielcba_usuario";

function obtenerToken() {
    try {
        return localStorage.getItem(CLAVE_TOKEN) || sessionStorage.getItem(CLAVE_TOKEN);
    } catch {
        return null;
    }
}

function guardarSesion(token, email, recordar) {
    cerrarSesion();
    const almacen = recordar ? localStorage : sessionStorage;
    almacen.setItem(CLAVE_TOKEN, token);
    almacen.setItem(CLAVE_USUARIO, email);
}

function cerrarSesion() {
    for (const almacen of [localStorage, sessionStorage]) {
        almacen.removeItem(CLAVE_TOKEN);
        almacen.removeItem(CLAVE_USUARIO);
    }
}

// Las páginas internas cargan este archivo en el <head>: sin token vuelven al login.
const esLogin = location.pathname.endsWith("/login.html");
if (!esLogin && !obtenerToken()) {
    location.replace("login.html");
}

// El botón "Cerrar sesión" borra el token antes de ir al login.
document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".riel-btn-logout").forEach((boton) =>
        boton.addEventListener("click", cerrarSesion)
    );
});
