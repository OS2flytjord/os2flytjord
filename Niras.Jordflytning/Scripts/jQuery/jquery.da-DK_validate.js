//Validering af dansk datoformat
$.validator.methods.date = function (value, element) {
  return this.optional(element) || (/^\d{1,2}[\/-]\d{1,2}[\/-]\d{4}(\s\d{2}:\d{2}(:\d{2})?)?$/.test(value)
      && !/Invalid|NaN/.test(new Date(value.replace("/", "-").split("-")[2], value.replace("/", "-").split("-")[1], value.replace("/", "-").split("-")[0])));
}
