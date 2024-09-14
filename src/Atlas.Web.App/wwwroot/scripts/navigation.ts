window.addEventListener('resize', () => {
  const elements = document.querySelectorAll('.expanded');

  if (!elements || elements.length === 0) {
    return;
  }

  elements.forEach(element => toggleExpand(element));

}, true);

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
