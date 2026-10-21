(function () {
    var SEGUNDOS = 3;
    var overlay = document.getElementById('transicion');
    var contador = document.getElementById('transicionSegundos');
    var activo = false;

    function iniciar(accion) {
        if (activo) return;
        activo = true;
        var restantes = SEGUNDOS;
        contador.textContent = restantes;
        overlay.hidden = false;
        var intervalo = setInterval(function () {
            restantes--;
            contador.textContent = Math.max(restantes, 0);
            if (restantes <= 0) {
                clearInterval(intervalo);
                accion();
            }
        }, 1000);
    }

    document.addEventListener('click', function (e) {
        if (e.defaultPrevented || e.button !== 0 || e.ctrlKey || e.shiftKey || e.metaKey || e.altKey) return;
        var a = e.target.closest('a[href]');
        if (!a) return;
        var href = a.getAttribute('href');
        if (!href || href.charAt(0) === '#' || href.indexOf('javascript:') === 0) return;
        if (a.target === '_blank' || a.hasAttribute('download') || a.hasAttribute('data-bs-toggle')) return;
        if (a.origin !== window.location.origin) return;
        e.preventDefault();
        iniciar(function () { window.location.href = a.href; });
    });

    document.addEventListener('submit', function (e) {
        var form = e.target;
        if (form.dataset.enviar === '1') return;
        if (e.defaultPrevented) return;
        if (window.jQuery && window.jQuery.fn.valid && window.jQuery(form).valid() === false) return;
        e.preventDefault();
        iniciar(function () {
            form.dataset.enviar = '1';
            form.submit();
        });
    });

    window.addEventListener('pageshow', function (e) {
        if (e.persisted) {
            overlay.hidden = true;
            activo = false;
        }
    });
})();
