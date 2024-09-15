window.addEventListener('DOMContentLoaded', () => {
  const size = getComputedStyle(document.body).getPropertyValue('--screen-md');

  const media = matchMedia(`(min-width: ${size})`);

  media.addEventListener("change", event => {
    if (!event.matches) {
      return;
    }

    const elements = document.querySelectorAll('.expanded');

    if (!elements || elements.length === 0) {
      return;
    }

    elements.forEach(element => toggleExpand(element));
  });
});

function toggleNavigation() {
  const header = document.querySelector('header');
  const hamburger = document.querySelector('.hamburger');

  if (!header || !hamburger) {
    return;
  }

  toggleExpand(header);
  toggleExpand(hamburger);
}

function toggleExpand(element: Element) {
  const expanded = element.getAttribute('aria-expanded') === 'true';

  element.classList.toggle('expanded');
  element.setAttribute('aria-expanded', String(!expanded));
}
