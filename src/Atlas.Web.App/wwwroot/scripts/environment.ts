window.addEventListener('DOMContentLoaded', () => {
    const url = window.location.hostname;
    const previewRegex = /^(?!atlas)([\w]+)\.([\w].+)$/;

    if (url.includes('localhost') || previewRegex.test(url)) {
        Blazor.start({
            environment: "Staging"
        });
    } else {
        Blazor.start();
    }
});
