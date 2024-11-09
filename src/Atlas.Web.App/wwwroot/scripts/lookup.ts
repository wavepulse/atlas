let eventCallback: (this: Document, ev: MouseEvent) => any;

function scrollToLookup() {
  const element = document.querySelector('.lookup-container');

  if (!element) {
    return;
  }

  setTimeout(() => {
    element.scrollIntoView({ behavior: 'smooth', block: 'center' });
  }, 300);
}

function addClearEvent(dotnet: DotNet) {
  eventCallback = (event: MouseEvent) => {
    const lookup = document.querySelector('.lookup-container');

    if (!event || !event.target || !(event.target instanceof Element)) {
      return;
    }

    if (event.target.classList.contains('item')) {
      return;
    }

    if (lookup && !lookup.contains(event.target)) {
      dotnet.invokeMethod('Clear');
    }
  };

  document.addEventListener('click', eventCallback);
}

function removeClearEvent() {
  document.removeEventListener('click', eventCallback);
}
