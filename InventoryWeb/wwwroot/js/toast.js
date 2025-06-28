window.showLiveToastById = function (ToastId) {
    const toastLive = document.getElementById(ToastId);
    if (toastLive) {
        const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLive);
        toastBootstrap.show();
    }
}