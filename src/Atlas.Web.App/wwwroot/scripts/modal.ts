function showModal(dialog: HTMLDialogElement) {
  dialog.showModal();
}

function closeModal(dialog: HTMLDialogElement) {
  dialog.close();
}

function addCloseOutsideEvent(dialog: HTMLDialogElement) {
  dialog.addEventListener('click', (event) => {
    if (event.target === dialog) {
      dialog.close();
    }
  });
}

function scrollContentToTop(dialog: HTMLDialogElement) {
  const content = dialog.querySelector('.content');

  content?.scrollTo({ top: 0, behavior: 'instant' });
}
