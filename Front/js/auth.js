// Sesión del front: guarda el token que devuelve POST /api/Auth/login.
// "Recordarme" lo deja en localStorage; si no, solo dura mientras esté abierta la pestaña.
const CLAVE_TOKEN = "rielcba_token";
const CLAVE_USUARIO = "rielcba_usuario";
const CLAVE_INVITADO = "rielcba_invitado";

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
        almacen.removeItem(CLAVE_INVITADO);
    }
}

// Sesión de invitado: entra sin cuenta y solo puede consultar (dura lo que la pestaña)
function esInvitado() {
    try {
        return sessionStorage.getItem(CLAVE_INVITADO) === "1";
    } catch {
        return false;
    }
}

function iniciarInvitado() {
    cerrarSesion();
    sessionStorage.setItem(CLAVE_TOKEN, "invitado");
    sessionStorage.setItem(CLAVE_USUARIO, "Invitado");
    sessionStorage.setItem(CLAVE_INVITADO, "1");
}

// Las páginas internas cargan este archivo en el <head>: sin token vuelven al login.
const esLogin = location.pathname.endsWith("/login.html");
if (!esLogin && !obtenerToken()) {
    location.replace("login.html");
}

// El invitado solo puede ver estas páginas; cualquier otra lo manda a Líneas.
const PAGINAS_INVITADO = ["lineas.html", "acerca.html"];
if (!esLogin && esInvitado() && !PAGINAS_INVITADO.some((p) => location.pathname.endsWith("/" + p))) {
    location.replace("lineas.html");
}

// El botón "Cerrar sesión" borra el token antes de ir al login.
document.addEventListener("DOMContentLoaded", () => {
    if (esInvitado()) {
        // el invitado solo ve Líneas y Acerca de: se sacan del menú las demás páginas
        document.querySelectorAll(".riel-navbar .nav-link").forEach((enlace) => {
            if (!PAGINAS_INVITADO.includes(enlace.getAttribute("href"))) enlace.closest("li")?.remove();
        });
        const marca = document.querySelector(".riel-navbar .navbar-brand");
        if (marca) marca.setAttribute("href", "lineas.html");
        document.querySelectorAll("[data-solo-registrados]").forEach((el) => el.remove());
        const salir = document.querySelector(".riel-btn-logout");
        if (salir) {
            const etiqueta = document.createElement("span");
            etiqueta.className = "badge rounded-pill text-bg-light";
            etiqueta.textContent = "Modo invitado";
            salir.before(etiqueta);
            salir.textContent = "Salir";
        }
    }
    document.querySelectorAll(".riel-btn-logout").forEach((boton) =>
        boton.addEventListener("click", cerrarSesion)
    );
});
