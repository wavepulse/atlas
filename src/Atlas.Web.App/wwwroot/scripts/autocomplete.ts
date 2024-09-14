function clearSearch(dotnet: any) {
  document.addEventListener('click', (event: MouseEvent) => {
    const autocomplete = document.querySelector('.autocomplete-container');

    if (!event || !event.target || !(event.target instanceof Element)) {
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

function scrollToAutocomplete() {
  const element = document.querySelector('.autocomplete-container');

  if (!element) {
    return;
  }

  const y = element.getBoundingClientRect().top + window.scrollY;

  window.scrollTo({ top: y, behavior: 'smooth' });
}

function focusOut(element: HTMLElement) {
  element.blur();
}
