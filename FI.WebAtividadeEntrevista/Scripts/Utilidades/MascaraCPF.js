
$(document).ready(function () {
    $('#CPF').on('input', function (e) {
        var target = $(this);
        var input = target.val().replace(/\D/g, '').substring(0, 11);
        var cpf = input.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
        target.val(cpf);
    })
});
