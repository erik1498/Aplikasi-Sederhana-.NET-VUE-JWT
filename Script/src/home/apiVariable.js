import BaseUrl from "../BaseUrl";

function baseHome(url = '') {
    return `${BaseUrl}/Home/${url}`;
}

export default baseHome;