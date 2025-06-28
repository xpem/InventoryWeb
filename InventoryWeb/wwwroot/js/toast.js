window.showLiveToastById = function (ToastId) {
    const toastLiveExample = document.getElementById(ToastId);
    if (toastLiveExample) {
        const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample);
        toastBootstrap.show();
    }
}