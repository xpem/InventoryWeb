const CACHE_VERSION = 'v2'; // Altere este valor a cada publica��o

// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
self.addEventListener('fetch', () => { });


self.addEventListener('install', event => { });