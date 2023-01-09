var nyrqUrl =
{
    set: function (addr) {
        const url = new URL(window.location.href);
        url.searchParams.set('addr', addr);
        window.history.replaceState(null, null, url);
    },
    get: function () {
        const url = new URL(window.location.href);
        var parameter = url.searchParams.get('addr');
        if (parameter == null || parameter == undefined) {
            return '';
        }
        else return parameter;
    }
} 