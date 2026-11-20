const modalOlvide = document.getElementById("modalOlvide");

document.getElementById("linkOlvide").addEventListener("click", (e) => {
    e.preventDefault();
    modalOlvide.showModal();
});

document.getElementById("cerrarOlvide").addEventListener("click", () => modalOlvide.close());
document.getElementById("aceptarOlvide").addEventListener("click", () => modalOlvide.close());

// click en el fondo oscuro cierra el popup
modalOlvide.addEventListener("click", (e) => {
    if (e.target === modalOlvide) modalOlvide.close();
});
