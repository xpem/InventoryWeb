// wwwroot/js/cameraInterop.js

window.cameraInterop = (function () {
    let cameraStream = null; // Para armazenar o objeto MediaStream

    // Helper para obter o elemento de vídeo
    function getVideoElement(id) {
        const video = document.getElementById(id);
        if (!video) {
            console.error('Elemento de vídeo não encontrado:', id);
        }
        return video;
    }

    // Helper para obter o elemento canvas
    function getCanvasElement(id) {
        const canvas = document.getElementById(id);
        if (!canvas) {
            console.error('Canvas element not found:', id);
            return null;
        }
        if (canvas.tagName.toLowerCase() !== 'canvas') {
            console.error(`Element with ID '${id}' is not a canvas element. Found: ${canvas.tagName}`);
            return null;
        }
        return canvas;
    }

    return {
        /**
         * Inicia o feed da câmera e o exibe em um elemento de vídeo.
         * @param {string} videoElementId O ID do elemento de vídeo.
         */
        startCameraFeed: async function (videoElementId) {
            const video = getVideoElement(videoElementId);
            if (!video) return;

            // Se já houver um stream ativo, pare-o primeiro para evitar múltiplos streams
            if (cameraStream) {
                cameraStream.getTracks().forEach(track => track.stop());
                video.srcObject = null;
                cameraStream = null;
            }

            try {
                // Solicita acesso à câmera (apenas vídeo)
                // 'environment' para câmera traseira, 'user' para câmera frontal
                cameraStream = await navigator.mediaDevices.getUserMedia({ video: { facingMode: 'environment' }, audio: false });
                video.srcObject = cameraStream;
                await video.play(); // Começa a tocar o feed de vídeo
                console.log("Feed da câmera iniciado.");
            } catch (err) {
                console.error('Erro ao acessar a câmera:', err);
                alert('Não foi possível acessar a câmera. Verifique as permissões do navegador e se HTTPS está habilitado. Erro: ' + err.name);
                cameraStream = null; // Garante que o stream seja nulo em caso de erro
            }
        },

        /**
         * Para o feed da câmera.
         * @param {string} videoElementId O ID do elemento de vídeo.
         */
        stopCameraFeed: function (videoElementId) {
            const video = getVideoElement(videoElementId);
            if (video && cameraStream) {
                cameraStream.getTracks().forEach(track => track.stop()); // Para todas as faixas (vídeo, áudio)
                video.srcObject = null; // Limpa a fonte do vídeo
                cameraStream = null; // Limpa a referência do stream
                console.log("Feed da câmera parado.");
            }
        },

        /**
         * Captura uma foto do feed de vídeo ativo e a retorna como um Data URL (Base64).
         * @param {string} videoElementId O ID do elemento de vídeo.
         * @param {string} canvasElementId O ID do elemento canvas para desenhar.
         * @returns {string | null} Os dados da imagem como um Data URL (ex: "data:image/png;base64,...").
         */
        capturePhoto: function (videoElementId, canvasElementId) {
            const video = getVideoElement(videoElementId);
            const canvas = getCanvasElement(canvasElementId);

            if (!video || !canvas || !cameraStream) {
                console.error('Vídeo, canvas ou stream da câmera não estão ativos ou encontrados.');
                alert('O feed da câmera não está ativo. Por favor, inicie a câmera primeiro.');
                return null;
            }

            // Define as dimensões do canvas para corresponder ao feed de vídeo
            canvas.width = video.videoWidth;
            canvas.height = video.videoHeight;

            const context = canvas.getContext('2d');
            if (!context) {
                console.error('Falha ao obter o contexto de renderização 2D para o canvas.');
                return null;
            }

            context.drawImage(video, 0, 0, canvas.width, canvas.height);

            const dataUrl = canvas.toDataURL('image/jpeg',0.8);

            console.log(dataUrl);
            if (dataUrl && dataUrl.startsWith('data:')) {
                return dataUrl;
            } else {
                console.error('Falha na criação do Data URL.');
                return null;
            }

        }
    };
})();