function clearSearch(dotNetReference) {
    document.addEventListener('click', (event) => {
        const autocomplete = document.querySelector('.autocomplete-container');

        if (event.target.classList.contains('autocomplete-item')) {
            return;
        }

        if (autocomplete && !autocomplete.contains(event.target)) {
            dotNetReference.invokeMethod('ClearSearch');
        }
    });
}
