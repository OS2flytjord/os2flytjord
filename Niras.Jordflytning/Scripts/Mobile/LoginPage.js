$(document).on("pageshow", "#loginPage", initLoginPage);

function initLoginPage() {
	Logger.log("initLoginPage!");
	$("#btnLogIndMobile").off('click');
	
	$("#btnLogIndMobile").click(function (e) {
		e.preventDefault();
		if ($("#txtUserName").val() != "" && $("#txtPassword").val() != "") {
			$.mobile.loading('show');
			$.post("/Mobile/Login", { brugernavn: $("#txtUserName").val(), password: $("#txtPassword").val() }, function (result) {

				if (result.Success) {
					document.location = "/Default/MobileMinSide/";
				} else {
					alert("Brugernavn eller password er forkert");
				}
				$.mobile.loading('hide');
			});
			return false;
		}
		$.mobile.loading('hide');
		return false;
	});

	//$("#loginForm").on("submit", function () {
	//	Logger.log("loginform submit!");

	//	if ($("#txtUserName").val() != "" && $("#txtPassword").val() != "") {
	//		$.mobile.loading('show');
	//		$.post("/Mobile/Login", { brugernavn: $("#txtUserName").val(), password: $("#txtPassword").val() }, function(result) {
				
	//			if (result.Success) {
	//				document.location = "/Default/MobileMinSide/";
	//			} else {
	//				alert("Brugernavn eller password er forkert");
	//			}
	//			$.mobile.loading('hide');
	//		});
	//		return false;
	//	}
	//	$.mobile.loading('hide');
	//	return false;
	//});
	
	
$("#btnResetPassword").click(function (e) {
	e.preventDefault();
	jQuery.ajax({
		url: '/Mobile/ResetPassword',
		method: 'GET',
		data: { email: $("#txtResetPasswordUsername").val() }
	}).done(function (response) {
		if (response.Success) {
			alert("Email, med dit nye password er afsendt");
			$.mobile.changePage(($("#loginPage")), { transition: "slide" });
		} else {
			alert("Der skete en fejl, tjek email adresse og prøv igen");
		}
		// Do something with the response
	}).fail(function () {
		alert("Der skete en fejl, tjek email adresse og prøv igen");
		// Whoops; show an error.
	});
});
}