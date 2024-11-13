function showModal() {
  const modal = document.querySelector<HTMLDialogElement>('.modal');

  modal?.showModal();
}

function closeModal() {
  const modal = document.querySelector<HTMLDialogElement>('.modal');
  modal?.close();
}
