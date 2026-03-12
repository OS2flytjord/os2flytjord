var Logger = Logger || {};
Logger.log = function (msg) {
	if (typeof console != "undefined") {
		console.log(msg);
	}
};

var tjekJordSvar; //Bruges til at gemme jordopslaget ved oprettelse af en sag.
var anvendOpretForm = false; //Bruges til at bestemme om der skal bruges data fra opret eller rediger anmeldelseformen ved tryk på indsend knap.

$(document).on("pageshow", "#anmeldelseRediger", loadAnmeldelse);
$(document).on("pageshow", "#anmeldelseOpretJordtjek", loadAnmeldelseOpretTjekjord);
$(document).on("pageshow", "#anmeldelseOpretTjekJordSvar", loadAnmeldelseOpretTjekjordSvar);
$(document).on("pageshow", "#anmeldelseOpret", loadAnmeldelseOpret);
$(document).on("pageshow", "#adgang", loadAdgang);
$(document).on("pageshow", "#minSide", initMinSide);
$(document).on("pageshow", "#minSideForside", initMinSideForside);
$(document).on("pageshow", "#previewAnmeldelse", initpreviewAnmeldelse);
//$('#anmeldelseOpret').on('pagehide', function () {
	
//	Logger.log(jQuery.contains(document, $('#anmeldelseOpret')[0]));
//	Logger.log(jQuery.contains(document, $('#partialMobileOpretAnmeldelse')[0]));
//	Logger.log(jQuery.contains(document, $('#partialMobileAnmeldelse')[0]));

//	$(this).remove();
//	$('#partialMobileOpretAnmeldelse').remove();
//	$('#partialMobileAnmeldelse').remove();
	
//	Logger.log(jQuery.contains(document, $('#anmeldelseOpret')[0]));
//	Logger.log(jQuery.contains(document, $('#partialMobileOpretAnmeldelse')[0]));
//	Logger.log(jQuery.contains(document, $('#partialMobileAnmeldelse')[0]));

//});
//$('#anmeldelseRediger').on('pagehide', function () {
	
//	Logger.log(jQuery.contains(document, $('#anmeldelseRediger')[0]));
//	Logger.log(jQuery.contains(document, $('#partialMobileOpretAnmeldelse')[0]));
//	Logger.log(jQuery.contains(document, $('#partialMobileAnmeldelse')[0]));

//	$(this).remove();
//	$('#partialMobileOpretAnmeldelse').remove(); 
//	$('#partialMobileAnmeldelse').remove();

//	Logger.log(jQuery.contains(document, $('#anmeldelseRediger')[0]));
//	 Logger.log(jQuery.contains(document, $('#partialMobileOpretAnmeldelse')[0]));
//	 Logger.log(jQuery.contains(document, $('#partialMobileAnmeldelse')[0]));
//});

//Bruger til at gemme anden betaler feltet hvis ikke der er valgt anden betaler som betaler.
function betalerChanged(e) {

	Logger.log($(e.target).val());
	Logger.log($(e.target).attr('id'));
	if ($(e.target).attr('id') == 'rbnBetaler_anden') {
		$("#andenBetalerDiv").show();
	} else {
		$("#andenBetalerDiv").hide();
	}
}


function anvenderFJChanged() {

	var anvenderFJ = $('#selectAnvenderFJ').val();
	var currentVal = $('#selectModtageranlaeg').val();
	if (currentVal == "" || currentVal == null) {
		 currentVal = ($("#hfModtagerAnlaegId").val());
	}

	if (anvenderFJ == "on") {
		$("#mobileAnmeldelseBetalerDiv").show();
	} else {
		$("#mobileAnmeldelseBetalerDiv").hide();
	}
	jQuery.ajax({
		url: '/Mobile/GetModtageranlaeg/',
		method: 'GET',
		data: { anvenderFJ: anvenderFJ, jordklassifikation: $('#hfForueningskategoriMobile').val() }
	}).done(function (result) {
		var options = $("#selectModtageranlaeg");
		options.empty();

		options.append($("<option />").val("").text("Vælg modtageanlæg"));

		$.each(result, function () {
			options.append($("<option />").val(this.value).text(this.text));
		});
		options.val(currentVal).selectmenu('refresh');
	}).fail(function () {

	});

}

function indeholderJordenAffaldChanged() {
	var sender = $("#IndeholderJordenAffald");
	$("#Affaldstype").selectmenu();
	if (sender.val() == "False") {
		$('#Affaldstype').selectmenu('disable');
		$("#AndetAffald").addClass('ui-disabled');
	} else if (sender.val() == "True") {
		$('#Affaldstype').selectmenu('enable');
		$("#AndetAffald").removeClass('ui-disabled');
	}

}

function bindVisAnmLinkHandlers() {
	$('a[data-anmid]', "#minSide").off('click');
	$('a[data-anmid]', "#minSide").click(function (e) {
		anmId = $(this).attr("data-anmid");
		$.mobile.changePage(($("#anmeldelseRediger")), { transition: "slide" });
	});
}

function bindVisAdgangAnmLinkHandlers() {
	$('a[data-adganganmid]', "#minSide").off('click');
	$('a[data-adganganmid]', "#minSide").click(function (e) {
		anmId = $(this).attr("data-adganganmid");
		$.mobile.changePage(($("#adgang")), { transition: "slide" });
	});
}

function initMinSide() {
	ddlAnmeldelserStatusOnChange();
	bindVisAnmLinkHandlers();
	bindVisAdgangAnmLinkHandlers();
	//$("#btnGemMinSide").off("click");
	//$("#btnGemMinSide").click(function (e) { gemAnmeldelse(); });
	//$("#btnOpretAnmeldelse").off("click");
	//$("#btnOpretAnmeldelse").click(function (e) { gemAnmeldelse(); });
}

function initMinSideForside() {
	$("#btnanmeldelseOpret").off('click');
	$("#btnanmeldelseOpret").click(function (e) {
		$.mobile.changePage(($("#anmeldelseOpretJordtjek")), { transition: "slide" });
	});
}

function selectModtageranlaegOnChange() {
	$("#hfModtagerAnlaegId").val($("#selectModtageranlaeg").val());
}

function selectTransportoerOnChange() {
	$("#hfTransportoerId").val($("#selectTransportoer").val());
}

function bindBetalerRBNs() {
	$("#rbnBetaler_anmelder").off('click');
	$("#rbnBetaler_anmelder").click(function (e) { betalerChanged(e); });
	$("#rbnBetaler_transportoer").off('click');
	$("#rbnBetaler_transportoer").click(function (e) { betalerChanged(e); });
	$("#rbnBetaler_anden").off('click');
	$("#rbnBetaler_anden").click(function (e) { betalerChanged(e); });
	
	
	
}

function loadAnmeldelse() {
	$.mobile.loading('show');
	//$.mobile.loadPage($('/Default/MobileMinSide#anmeldelseRediger'));
	$("#partialMobileAnmeldelse").load("/Mobile/_MobileAnmeldelse?anmeldelseId=" + anmId, function () {
		// evt. callback kald


		$("#btnAnmeldelseGem").off("click");
		$("#btnAnmeldelseGem").click(function (e) { gemAnmeldelse(); });

		$('#DatoTil').datepicker();
		$('#DatoFra').datepicker();
		$("#IndeholderJordenAffald").off('change');
		$("#IndeholderJordenAffald").change(function (e) { indeholderJordenAffaldChanged(); });
		$("#selectAnvenderFJ").off('change');
		$("#selectAnvenderFJ").change(function (e) { anvenderFJChanged(); });
		bindBetalerRBNs();
		
		//manuelt sætte om anden betaler skal vises alt efter hvem der er valgt som betaler
 
		if ($("#rbnBetaler_anden").is(':checked')) {
			$("#andenBetalerDiv").show();
		} else {
			$("#andenBetalerDiv").hide();
		}

		setAnmeldelseJordforureningRadiobuttonsOnChange();
		//$('selectModtageranlaeg').val($("#hfModtagerAnlaegId").val());

		$("#txtareaAdresse").attr('readonly', 'readonly');
		$("#Anmeldelse_Oprindelsessted_Postnummer").attr('readonly', 'readonly');
		$("#Anmeldelse_Kommune_Navn").attr('readonly', 'readonly');


		if ($('#hfAnvenderFJ').val() == "True") {
			$('#selectAnvenderFJ').val("on");
		} else {
			$('#selectAnvenderFJ').val("off");
		}
		anvenderFJChanged();
		$('selectModtageranlaeg').val($("#hfModtagerAnlaegId").val());


		indeholderJordenAffaldChanged();
		$("#datepickerStart").datepicker($.datepicker.regional["da"]);
		$("#datepickerSlut").datepicker($.datepicker.regional["da"]);
		$("#btnPreviewAnmeldelse").off('click');
		$("#btnPreviewAnmeldelse").click(function (e) {
			previewAnmeldelse();
		});
		$("#partialMobileAnmeldelse").trigger("create");
		$('#suggestions').listview();
		$("#searchField").autocomplete({
			target: $("#suggestions"),
			source: '/mobile/SoegAndenBetaler',
			callback: function (e) {
				var $a = $(e.currentTarget); // access the selected item
				$("#searchField").val($a.text()); // place the value of the selection into the search box
				$("#searchField").autocomplete("clear"); // clear the listview
			},
			minLength: 3
		});
		$.mobile.loading('hide');
	});
}

function getJordForureningMinSideOpretAnmeldelse() {
	$.mobile.loading('show');
	var result;
	jQuery.ajax({
		url: '/Mobile/GetJordforurening',
		method: 'GET',
		data: { wkt: valgtSted.wkt, kommunenavn: valgtSted.kommunenavn } //$('#redigerAnmeldelsefrm').serialize()
	}).done(function (response) {
		$.mobile.loading('hide');
		if (response) {
		
			$("#anmeldelseOpretTjekJordSvarPartial").html(response).trigger('create'); //Erstatter alt inde for div'en med det partial view som man ved GetJordForurening
			$.mobile.changePage(($("#anmeldelseOpretTjekJordSvar")), { transition: "slide" });

		}
	}).fail(function () {
		$.mobile.loading('hide');
		result = false;
		alert("fatal error!");
		// Whoops; show an error.
	});

}


function loadAnmeldelseOpretTjekjordSvar() {
	$("#btnOpretAnmeldelseTjekJordSvar").off('click');
	$("#btnOpretAnmeldelseTjekJordSvar").click(function (e) {
		e.preventDefault();
	
		jQuery.ajax({
			url: '/Validation/MobileCanCreateAnmeldelse',
			method: 'GET',
			data: { kommunenavn: valgtSted.kommunenavn }
		}).done(function (response) {

			if (response == true || response == "true") {
				
				//Logger.log(jQuery.contains(document, $('#anmeldelseRediger')[0]));
				//Logger.log(jQuery.contains(document, $('#partialMobileOpretAnmeldelse')[0]));
				//Logger.log(jQuery.contains(document, $('#partialMobileAnmeldelse')[0]));

				//$(this).remove();
				//$('#partialMobileOpretAnmeldelse').remove();
				//$('#partialMobileAnmeldelse').remove();
				$('#anmeldelseContainerDiv').remove();
				
				//Logger.log(jQuery.contains(document, $('#anmeldelseRediger')[0]));
				//Logger.log(jQuery.contains(document, $('#partialMobileOpretAnmeldelse')[0]));
				//Logger.log(jQuery.contains(document, $('#partialMobileAnmeldelse')[0]));

				anmId = undefined;
				$.mobile.changePage(($("#anmeldelseOpret")), { transition: "slide" });			
			} else {
				alert("Det er ikke muligt at oprette en anmeldese på denne adresse da kommunen ikke anvender flytjord.\nDu kan oprette en anmeldelse ved at andvende desktopudgaven af flytjord.dk");
			}

		}).fail(function () {
			alert("Der er sket en fejl!");
		});
	});

}

function loadAnmeldelseOpretTjekjord() {

	$("#mobbileAdresseTjekDiv").hide();
	$("#mobbileAdresseTjekOretDiv").show();

	//	initializeMap();
        init();
		$("#adresseSearchField").val("");
		$("#husnrSearchField").val("");
		$("#btnOpretAnmeldelseJordtjekVidere").off('click');
		$("#btnOpretAnmeldelseJordtjekVidere").click(function (e) {
			e.preventDefault();
			if (valgtSted != null) {
				getJordForureningMinSideOpretAnmeldelse();
			} else {
				alert("Vælg en adresse først..");
			}
		});
		$('#adresseSuggestions').listview();
		$("#adresseSearchField").autocomplete({
			target: $("#adresseSuggestions"),
			source: '/Adresse/SearchVejeMobile',
			loadingHtml: "Søger...",
			callback: function(e) {
				var $a = $(e.currentTarget); // access the selected item
				$("#adresseSearchField").val($a.text()); // place the value of the selection into the search box
				$("#adresseSearchField").autocomplete("clear"); // clear the listview

				var jsonObj = jQuery.parseJSON($a.attr('data-autocomplete'));
				var vejkode = jsonObj.vejkode;
				var postnr = jsonObj.postnr;
				loadHusnumrene(vejkode, postnr);
				$('#husnrSuggestions').empty();
				$("#husnrSearchField").focus();
			},
			minLength: 4
		});
		$("#adresseSearchField").autocomplete("option", "delay", 500);
}

function loadAnmeldelseOpret() {
	$.mobile.loading('show');

	jQuery.ajax({
		url: '/Mobile/_MobileAnmeldelse',
		method: 'GET',
		data: { kommunenavn: valgtSted.kommunenavn } //$('#redigerAnmeldelsefrm').serialize()
	}).done(function (response) {
		$.mobile.loading('hide');
		if (response) {

			$("#partialMobileOpretAnmeldelse").html(response).trigger('create');

			tjekJordSvar = $("#hfForureningskodeTjekJordSvar").val();
			Logger.log("jordklasseguid:");
			Logger.log(tjekJordSvar);
			$("#hfForueningskategoriMobile").val(tjekJordSvar);
			Logger.log('#hfForueningskategoriMobile');

			//$("#hfForueningskategoriMobile").val(tjekJordSvar).change();
			//$(".jordklassifikationTypeRadiolist").prop("checked", false).checkboxradio('disable').checkboxradio("refresh");

			$(".jordklassifikationTypeRadiolist").prop("checked", false).checkboxradio("refresh");
			//$('input:radio[id="Forueningskategori_kraftigt_forurenet_jord"]').prop("checked", false).checkboxradio('disable').checkboxradio("refresh");
			//$('input:radio[id="Forueningskategori_let_forurenet_jord"]').prop("checked", false).checkboxradio('disable').checkboxradio("refresh");
			//$('input:radio[id="Forueningskategori_ren_jord"]').prop("checked", false).checkboxradio('disable').checkboxradio("refresh");



			$("#btnAnmeldelseGem").off("click");
			$("#btnAnmeldelseGem").click(function (e) { gemAnmeldelse(); });


			//$("input:radio").each(function () {
			//	Logger.log("lala");
			//	Logger.log($(this).val() + "");
			//	if ($(this).val() + "" == tjekJordSvar + "") {
			//		$(this).prop('checked', true);
			//	}
			//});

			//$("input[name=Forueningskategori]:radio").each(function () {
			//	Logger.log("lala");
			//	Logger.log($(this).val() + "");
			//	if ($(this).val() + "" == tjekJordSvar + "") {
			//		$(this).prop('checked', true);
			//	}
			//});


			//$('input:radio[class=jordklassifikationTypeRadiolist]').each(function () {
			//	Logger.log("lala");
			//	Logger.log($(this).val() + "");
			//	if ($(this).val() + "" == tjekJordSvar + "") {
			//		$(this).prop('checked', true);
			//	}
			//});

			$(".jordklassifikationTypeRadiolist").each(function () {			
				if ($(this).val()+ "" == tjekJordSvar + "") {
					$(this).prop('checked', true);
				}
			});

			setAnmeldelseJordforureningRadiobuttonsOnChange();
			//if (tjekJordSvar == "1") {

			//	$('input:radio[id="Forueningskategori_kraftigt_forurenet_jord"]').prop("checked", true).checkboxradio('disable').checkboxradio("refresh");
			//} else if (tjekJordSvar == "2") {

			//	$('input:radio[id="Forueningskategori_let_forurenet_jord"]').prop("checked", true).checkboxradio('disable').checkboxradio("refresh");
			//}
			//else if (tjekJordSvar == "3") {

			//	$('input:radio[id="Forueningskategori_ren_jord"]').prop("checked", true).checkboxradio('disable').checkboxradio("refresh");
			//}


			$("#hfForueningskategoriMobile").val(tjekJordSvar);
			$("#txtareaAdresse").val(valgtSted.vejnavn + " " + valgtSted.husnr).attr('readonly', 'readonly');
			$("#Anmeldelse_Oprindelsessted_Postnummer").val(valgtSted.postnummer).attr('readonly', 'readonly');
			$("#Anmeldelse_Kommune_Navn").val(valgtSted.kommunenavn).attr('readonly', 'readonly');
			$('#hfMobilePostDistrikt').val(valgtSted.postdistrikt);
			$("#btnOpretAnmeldelse").off("click");
			$("#btnOpretAnmeldelse").click(function (e) { opretAnmeldelse(); });
			$("#hideAndSeekDiv").hide();

			$('#DatoTil').datepicker();
			$('#DatoFra').datepicker();
			$("#IndeholderJordenAffald").off('change');
			$("#IndeholderJordenAffald").change(function (e) { indeholderJordenAffaldChanged(); });
			$("#selectAnvenderFJ").off('change');
			$("#selectAnvenderFJ").change(function (e) { anvenderFJChanged(); });

			bindBetalerRBNs();

			//manuelt sætte om anden betaler skal vises alt efter hvem der er valgt som betaler

			if ($("#rbnBetaler_anden").is(':checked')) {
				$("#andenBetalerDiv").show();
			} else {
				$("#andenBetalerDiv").hide();
			}
			$("#btnPreviewAnmeldelse").off('click');
			$("#btnPreviewAnmeldelse").click(function (e) {
				previewAnmeldelse();
			});
			anvenderFJChanged();
			indeholderJordenAffaldChanged();
			$("#datepickerStart").datepicker($.datepicker.regional["da"]);
			$("#datepickerSlut").datepicker($.datepicker.regional["da"]);
			$("#partialMobileOpretAnmeldelse").trigger("create");
			$('#suggestions').listview();
			$("#searchField").autocomplete({
				target: $("#suggestions"),
				source: '/mobile/SoegAndenBetaler',
				callback: function (e) {
					var $a = $(e.currentTarget); // access the selected item
					$("#searchField").val($a.text()); // place the value of the selection into the search box
					$("#searchField").autocomplete("clear"); // clear the listview
				},
				minLength: 3
			});

			$("#hfForueningskategoriMobile").val(tjekJordSvar);
			$("#txtareaAdresse").val(valgtSted.vejnavn + " " + valgtSted.husnr).attr('readonly', 'readonly');
			$("#Anmeldelse_Oprindelsessted_Postnummer").val(valgtSted.postnummer).attr('readonly', 'readonly');
			$("#Anmeldelse_Kommune_Navn").val(valgtSted.kommunenavn).attr('readonly', 'readonly');
			$('#hfMobilePostDistrikt').val(valgtSted.postdistrikt);

			$.mobile.loading('hide');


			//$("#anmeldelseOpretTjekJordSvarPartial").html(response).trigger('create'); //Erstatter alt inde for div'en med det partial view som man ved GetJordForurening
			//$.mobile.changePage(($("#anmeldelseOpretTjekJordSvar")), { transition: "slide" });

		}
	}).fail(function () {
		$.mobile.loading('hide');
		//result = false;
		alert("fatal error!");
		// Whoops; show an error.
	});



	//$("#partialMobileOpretAnmeldelse").load("/Mobile/_MobileAnmeldelse?kommunenavn=" + valgtSted.kommunenavn, function () {
	//	// evt. callback kald
	//	$("#partialMobileOpretAnmeldelse").trigger('create');
	//	tjekJordSvar = $("#hfForureningskodeTjekJordSvar").val();
	//	Logger.log(tjekJordSvar);
	//	//$("#hfForueningskategoriMobile").val(tjekJordSvar).change();
	//	//$(".jordklassifikationTypeRadiolist").prop("checked", false).checkboxradio('disable').checkboxradio("refresh");

	//	$(".jordklassifikationTypeRadiolist").prop("checked", false).checkboxradio("refresh");
	//	//$('input:radio[id="Forueningskategori_kraftigt_forurenet_jord"]').prop("checked", false).checkboxradio('disable').checkboxradio("refresh");
	//	//$('input:radio[id="Forueningskategori_let_forurenet_jord"]').prop("checked", false).checkboxradio('disable').checkboxradio("refresh");
	//	//$('input:radio[id="Forueningskategori_ren_jord"]').prop("checked", false).checkboxradio('disable').checkboxradio("refresh");
	


	//	$("#btnAnmeldelseGem").off("click");
	//	$("#btnAnmeldelseGem").click(function (e) { gemAnmeldelse(); });
		
		
	//	$("input:radio").each(function () {
	//		Logger.log("lala");
	//		Logger.log($(this).val() + "");
	//		if ($(this).val() + "" == tjekJordSvar + "") {
	//			$(this).prop('checked', true);
	//		}
	//	});

	//	$("input[name=Forueningskategori]:radio").each(function () {
	//		Logger.log("lala");
	//		Logger.log($(this).val() + "");
	//		if ($(this).val() + "" == tjekJordSvar + "") {
	//			$(this).prop('checked', true);
	//		}
	//	});


	//	$('input:radio[class=jordklassifikationTypeRadiolist]').each(function () {
	//		Logger.log("lala");
	//		Logger.log($(this).val() + "");
	//		if ($(this).val() + "" == tjekJordSvar + "") {
	//			$(this).prop('checked', true);
	//		}
	//	});

	//	//$(".jordklassifikationTypeRadiolist").each(function () {			
	//	//	Logger.log("lala");
	//	//	Logger.log($(this).val() + "");
	//	//	if ($(this).val()+ "" == tjekJordSvar + "") {
	//	//		$(this).prop('checked', true);
	//	//	}
	//	//});

	//	setAnmeldelseJordforureningRadiobuttonsOnChange();
	//		//if (tjekJordSvar == "1") {

	//		//	$('input:radio[id="Forueningskategori_kraftigt_forurenet_jord"]').prop("checked", true).checkboxradio('disable').checkboxradio("refresh");
	//		//} else if (tjekJordSvar == "2") {

	//		//	$('input:radio[id="Forueningskategori_let_forurenet_jord"]').prop("checked", true).checkboxradio('disable').checkboxradio("refresh");
	//		//}
	//		//else if (tjekJordSvar == "3") {

	//		//	$('input:radio[id="Forueningskategori_ren_jord"]').prop("checked", true).checkboxradio('disable').checkboxradio("refresh");
	//		//}

	//	Logger.log(valgtSted);
	//	Logger.log('kfk test');
	//	Logger.log(tjekJordSvar);
	//	$("#hfForueningskategoriMobile").val(tjekJordSvar);
	//	$("#txtareaAdresse").val(valgtSted.vejnavn + " " + valgtSted.husnr).attr('readonly', 'readonly');
	//	$("#Anmeldelse_Oprindelsessted_Postnummer").val(valgtSted.postnummer).attr('readonly', 'readonly');
	//	$("#Anmeldelse_Kommune_Navn").val(valgtSted.kommunenavn).attr('readonly', 'readonly');
	//	$('#hfMobilePostDistrikt').val(valgtSted.postdistrikt);
	//	$("#btnOpretAnmeldelse").off("click");
	//	$("#btnOpretAnmeldelse").click(function (e) { opretAnmeldelse(); });
	//	$("#hideAndSeekDiv").hide();

	//	$('#DatoTil').datepicker();
	//	$('#DatoFra').datepicker();
	//	$("#IndeholderJordenAffald").off('change');
	//	$("#IndeholderJordenAffald").change(function (e) { indeholderJordenAffaldChanged(); });
	//	$("#selectAnvenderFJ").off('change');
	//	$("#selectAnvenderFJ").change(function (e) { anvenderFJChanged(); });	

	//	bindBetalerRBNs();

	//	//manuelt sætte om anden betaler skal vises alt efter hvem der er valgt som betaler

	//	if ($("#rbnBetaler_anden").is(':checked')) {
	//		$("#andenBetalerDiv").show();
	//	} else {
	//		$("#andenBetalerDiv").hide();
	//	}
	//	$("#btnPreviewAnmeldelse").off('click');
	//	$("#btnPreviewAnmeldelse").click(function (e) {
	//		previewAnmeldelse();
	//	});
	//	anvenderFJChanged();
	//	indeholderJordenAffaldChanged();
	//	$("#datepickerStart").datepicker($.datepicker.regional["da"]);
	//	$("#datepickerSlut").datepicker($.datepicker.regional["da"]);
	//	$("#partialMobileOpretAnmeldelse").trigger("create");
	//	$('#suggestions').listview();
	//	$("#searchField").autocomplete({
	//		target: $("#suggestions"),
	//		source: '/mobile/SoegAndenBetaler',
	//		callback: function (e) {
	//			var $a = $(e.currentTarget); // access the selected item
	//			$("#searchField").val($a.text()); // place the value of the selection into the search box
	//			$("#searchField").autocomplete("clear"); // clear the listview
	//		},
	//		minLength: 3
	//	});
		
	//	$("#hfForueningskategoriMobile").val(tjekJordSvar);
	//	$("#txtareaAdresse").val(valgtSted.vejnavn + " " + valgtSted.husnr).attr('readonly', 'readonly');
	//	$("#Anmeldelse_Oprindelsessted_Postnummer").val(valgtSted.postnummer).attr('readonly', 'readonly');
	//	$("#Anmeldelse_Kommune_Navn").val(valgtSted.kommunenavn).attr('readonly', 'readonly');
	//	$('#hfMobilePostDistrikt').val(valgtSted.postdistrikt);

	//	$.mobile.loading('hide');
	//});
}

function loadAdgang() {

	$("#partialMobileAdgang").load("/Mobile/MobileAdgang?anmeldelseId=" + anmId, function() {
		// evt. callback kald

		jQuery.ajax({
			url: '/Mobile/GetLoebenummer',
			method: 'GET',
			data: { anmeldelsesId: anmId }
		}).done(function(response) {
			Logger.log(response);
			//https://github.com/jeromeetienne/jquery-qrcode
			$('#qrcodeholder').qrcode({
				text: "1"+response,
				render: "canvas",  // 'canvas' or 'table'. Default value is 'canvas'
				background: "#ffffff",
				foreground: "#000000",
				width: 100,
				height: 100
			});
		});

	});
}

function previewAnmeldelse() {

	Logger.log(typeof anmId);
	Logger.log(anmId);

	if (typeof anmId == 'undefined') {		
		jQuery.ajax({
			url: '/Mobile/SaveAnmeldelse',
			method: 'GET',
			data: $('#opretAnmeldelsefrm').serialize()
		}).done(function (response) {
		  if (response.Success && response.anmeldelseId != "00000000-0000-0000-0000-000000000000") {
		    anmId = response.anmeldelseId;
		    $("#hfAnmeldelseId").val(response.anmeldelseId);
		    Logger.log("oprettrue");
				anvendOpretForm = true;
				gotoPreviewAnmeldelse();
		  } else {
		    if (response.Message != "") {
		      alert(response.Message);
		    } else {
		      alert("Der er sket en fejl!");
		    }
		  }
			// Do something with the response
		}).fail(function () {
			alert("Der er sket en fejl!");
			// Whoops; show an error.
		});
	} else {
		Logger.log("opretfalse");
		anvendOpretForm = false;
		gotoPreviewAnmeldelse();
	}
}

function gotoPreviewAnmeldelse()
{
	$("#previewAnmeldelsePartial").load("/Mobile/PreviewAnmeldelse?anmeldelseId=" + anmId, function () {
		// evt. callback kald
		$("#btnKvittering").click(function (e) {
			loadKvittering();
		});

		$('#previewAnmeldelse').trigger('create');
		$('#btnKvittering').button();
		$.mobile.changePage(($("#previewAnmeldelse")), { transition: "slide" });
		//changepage??
		//tilknyt initKvittering
	});
}

function initpreviewAnmeldelse() {
		$('#previewAnmeldelse').trigger('create');
		$('#btnKvittering').button();

}




function gemAnmeldelse() {
	var data;
	if ($("#hfAnmeldelseId").val() == "00000000-0000-0000-0000-000000000000") {
		data = $('#opretAnmeldelsefrm').serialize();
	} else {
	  data = $('#redigerAnmeldelsefrm').serialize();
	  if (data == "") {
	    //Man er altså ikke i redigerAnmeldelsefrm, da der ikke er nogen data. 
	    //Ved ikke hvordan KFK havde tænkt det, men jeg lavet nedenstående fix. \KVE 2014 01 15
	    data = $('#opretAnmeldelsefrm').serialize();
	  }
	}

	jQuery.ajax({
		url: '/Mobile/SaveAnmeldelse',
		method: 'GET',
		data: data
	}).done(function (response) {
		if (response.Success) {
		  alert("Anmeldelsen er gemt");
		  $("#hfAnmeldelseId").val(response.anmeldelseId);
		} else {

		  if (response.Message != "") {
		    alert(response.Message);
		  } else {
		    alert("Der er sket en fejl!");
		  }
			
		}
		// Do something with the response
	}).fail(function () {
		alert("Der er sket en fejl!");
		// Whoops; show an error.
	});
}

function opretAnmeldelse() {

	
		jQuery.ajax({
			url: '/Mobile/SaveAnmeldelse',
			method: 'GET',
			data: $('#opretAnmeldelsefrm').serialize()
		}).done(function (response) {
		  if (response.Success) {
		    $("#hfAnmeldelseId").val(response.anmeldelseId);
				alert("Anmeldelsen er oprettet");
				$.mobile.changePage(($("#minSide")), { transition: "slide" });
			} else {

		    if (response.Message != "") {
		      alert(response.Message);
		    } else {
		      alert("Der er sket en fejl!");
		    }

			}
			// Do something with the response
		}).fail(function () {
			alert("Der er sket en fejl!");
			// Whoops; show an error.
		});
}


function loadKvittering() {

	var data;
	if (anvendOpretForm) {
		data = $('#opretAnmeldelsefrm').serialize();
	} else {
		data = $('#redigerAnmeldelsefrm').serialize();
	}

	jQuery.ajax({
		url: '/Mobile/IndsendAnmeldelse',
		method: 'GET',
		data: data //$('#redigerAnmeldelsefrm').serialize()
	}).done(function (response) {
		if (response.Success) {

			$("#partialKvittering").load("/Mobile/Kvittering?anmeldelseId=" + anmId, function () {
				// evt. callback kald	
				anmId = undefined;
				$.mobile.changePage(($("#kvittering")), { transition: "slide" });
			});

		} else {
			//Vis messagebox med valideringsfejl
			$('#generic-dialog').on('pagebeforeshow', function (event, ui) {
				$('#the-content').html(response.Validering).trigger('create');

			});
			$.mobile.changePage("#generic-dialog");

		}
		// Do something with the response
	}).fail(function () {
		alert("fatal error!");
		// Whoops; show an error.
	});
}

function ddlAnmeldelserStatusOnChange() {

	var sender = $("#ddlAnmeldelserStatus");
	var senderValue = sender.val();
	var output = '';
	$("#mobileAnmeldelser").html(output);
	jQuery.ajax({
		url: '/Mobile/GetBrugerAnmeldelser',
		method: 'GET',
		data: { type: senderValue }
	}).done(function (response) {
		if (response) {
			if (senderValue == "aktive") {
				for (key in response) {
					output += '<div data-role="collapsible"><h3>' + response[key].Visningstekst + '</h3>' + '<ul data-role="listview" data-divider-theme="b" data-inset="true"><li data-theme="c"><a href="#anmeldelseRediger" data-anmid=' + response[key].Id + ' data-transition="slide">Se anmeldelse</a></li><li data-theme="c"><a href="#adgang" data-adganganmid=' + response[key].Id + ' data-transition="slide">Adgang med QR-kode</a></li></ul></div>';
					//	output += '<li>' + data[key].title + ' (Added using for())</li>';
				}
			}		
			else if (senderValue == "afsluttede") {
				for (key in response) {
					output += '<div data-role="collapsible"><h3>' + response[key].Visningstekst + '</h3>' + '<ul data-role="listview" data-divider-theme="b" data-inset="true"><li data-theme="c"><a href="#anmeldelseRediger" data-anmid=' + response[key].Id + ' data-transition="slide">Se anmeldelse</a></li></ul></div>';
					//	output += '<li>' + data[key].title + ' (Added using for())</li>';
				}
			} else if (senderValue == "fremsendte") {
				for (key in response) {
					output += '<div data-role="collapsible"><h3>' + response[key].Visningstekst + '</h3>' + '<ul data-role="listview" data-divider-theme="b" data-inset="true"><li data-theme="c"><a href="#anmeldelseRediger" data-anmid=' + response[key].Id + ' data-transition="slide">Se anmeldelse</a></li></ul></div>';
					//	output += '<li>' + data[key].title + ' (Added using for())</li>';
				}
			}
			else if (senderValue == "kladder") {
				for (key in response) {
					output += '<div data-role="collapsible"><h3>' + response[key].Visningstekst + '</h3>' + '<ul data-role="listview" data-divider-theme="b" data-inset="true"><li data-theme="c"><a href="#anmeldelseRediger" data-anmid=' + response[key].Id + ' data-transition="slide">Se anmeldelse</a></li></ul></div>';
					//	output += '<li>' + data[key].title + ' (Added using for())</li>';
				}
			}
			$("#mobileAnmeldelser").html(output).trigger('create');
			bindVisAnmLinkHandlers();
			if (senderValue == "aktive") {
				bindVisAdgangAnmLinkHandlers();
			}
		} else {
			alert("Der skete en fejl");
		}
		// Do something with the response
	}).fail(function () {
		alert("Der er sket en fejl!");
		// Whoops; show an error.
	});
}

function isEmailUniqueMobile(email) {
	$.ajax({
		// This is key, otherwise request will be made asynchronously and you won't get your response from the server when needed
		async: false,
		type: 'GET',
		url: '@Url.Action("IsUserIdAvailable", "Validation", new { area = "" })',
		data: { Email: email }
	})
		// My action is a JsonResult that returns either true or false (no quotes, etc.)
		.done(function (data) {
			isValid = data;
		})
		.fail(function (request, status, error) {
			isValid = false;
		});
	return isValid;
}

function setAnmeldelseJordforureningRadiobuttonsOnChange() {
	
	$(".jordklassifikationTypeRadiolist").each(function () {
		$(this).off('change');
		$(this).change(function () {
			rbtChangeFunction($(this));
		});
	});

}

//Write Your Code Here
	
function rbtChangeFunction(elem) {
		if(elem.is(':checked')) {
			$('#hfForueningskategoriMobile').val(elem.val());
			anvenderFJChanged();
		}
	}

	