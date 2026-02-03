window.blazorCulture = {
    get: function () {
        const name = '.AspNetCore.Culture';
        const match = document.cookie
            .split('; ')
            .find(row => row.startsWith(name + '='));

        if (!match) return null;

        const value = decodeURIComponent(match.split('=')[1]);
        // value looks like: c=fr-CA|uic=fr-CA

        const cultureMatch = value.match(/c=([^|]+)/);
        return cultureMatch ? cultureMatch[1] : null;
    }
};
