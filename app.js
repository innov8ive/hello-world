
let extras = {
'method': 'get',
'params': {
	'cid': "5BH5FIMSITMZ0ZJ5V8BS8V093L702DNX",
	'appId': "VA89480XOUGMECS1MU56"
    }
};
manager.showFabButton(false);
const data = await manager.showOtplessLoginPage(extras);

let message: string = '';
if (data.data === null || data.data === undefined) {
message = data.errorMessage;
} else {
message ="token: ${data.data.token}";
// todo here
}
VA89480XOUGMECS1MU56
5BH5FIMSITMZ0ZJ5V8BS8V093L702DNX
sb7muh67z6d8vcxgq6ynvg4zqrzyu89f

