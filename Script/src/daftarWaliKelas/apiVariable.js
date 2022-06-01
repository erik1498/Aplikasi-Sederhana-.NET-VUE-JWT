import BaseUrl from "../BaseUrl";

function baseWaliKelas(url = '') {
    return `${BaseUrl}/Jurusan/${url}`;
}

export default baseWaliKelas;