declare var Blazor: {
    start: (options?: { environment?: string }) => void;
};

window.addEventListener('DOMContentLoaded', () => {
    const url = window.location.hostname;
    const previewRegex = /^([a-zA-Z0-9]+)\.([\w].+)$/;

    if (url.includes('localhost') || previewRegex.test(url)) {
        Blazor.start({
            environment: "Staging"
        });
    } else {
        Blazor.start();
    }
});
