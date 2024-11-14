function showModal() {
  const modal = document.querySelector<HTMLDialogElement>('.modal');

  modal?.showModal();
}

function closeModal() {
  const modal = document.querySelector<HTMLDialogElement>('.modal');
  modal?.close();
}

function scrollContentToTop() {
  const content = document.querySelector('.content');

  content?.scrollTo({ top: 0, behavior: 'instant' });
}
