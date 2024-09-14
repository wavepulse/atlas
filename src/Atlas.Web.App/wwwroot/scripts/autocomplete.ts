function clearSearch(dotnet: any) {
  document.addEventListener('click', (event: MouseEvent) => {
    const autocomplete = document.querySelector('.autocomplete-container');

    if (!event || !event.target || !(event.target instanceof HTMLElement)) {
      return;
    }

    if (event.target.classList.contains('autocomplete-item')) {
      return;
    }

    if (autocomplete && !autocomplete.contains(event.target)) {
      dotnet.invokeMethod('ClearSearch');
    }
  });
}

function focusOut(element: HTMLElement) {
  element.blur();
}
