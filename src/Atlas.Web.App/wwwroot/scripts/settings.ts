function changeTheme(theme: string) {
    const main = document.querySelector('.main-layout');

    main?.setAttribute('data-theme', theme);
}
