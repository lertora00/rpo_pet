$(document).ready(initTheme);

function initTheme() {
    selectMenu();
}

function selectMenu() {
    var menu = location.href.substring(7);
    menu = menu.substring(menu.indexOf('/'));
    var i = menu.indexOf('#');
    if (i > -1) menu = menu.substring(0, i);
    $("ul.menu-links a[href='" + menu + "']").parent().addClass('active');
}