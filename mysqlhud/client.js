/// <reference types="@altv/types-client" />
/// <reference types="@altv/types-natives" />

import * as alt from 'alt-client';
import * as native from 'natives';

const player = alt.Player.local;
let loginView;

alt.on('connectionComplete', ()  => {
    loginView = new alt.WebView("http://resource/login/index.html");
    loginView.focus();

    alt.showCursor(true);
    alt.toggleGameControls(false);
    native.doScreenFadeOut(0);

    loginView.on('mysql:login', (name, password) => {
        alt.emitServer('mysql:loginAttempt', name, password);
    });

    loginView.on('mysql:register', (name, password) => {
        alt.emitServer('mysql:registerAttempt', name, password);
    });
})

alt.onServer('mysql:loginSuccess', () => {
    alt.showCursor(false);
    alt.toggleGameControls(true);
    native.doScreenFadeIn(1000);
    if(loginView) loginView.destroy();
})

alt.onServer('mysql:loginError', (type, msg) => {
    if(loginView) loginView.emit('showError', type, msg);
})

alt.onServer('mysql:notify', (msg) => {
    const textEntry = `TEXT_ENTRY_${(Math.random() * 1000).toFixed(0)}`;
    alt.addGxtText(textEntry, msg);
    native.beginTextCommandThefeedPost('STRING');
    native.addTextComponentSubstringTextLabel(textEntry);
    native.addTextCommandThefeedPostTicker(false, false);
})