var beneficiarios = [];

$(document).ready(function () {
    $('#beneficiarioCPF').on('input', function (e) {
        var target = $(this);
        var input = target.val().replace(/\D/g, '').substring(0, 11);
        var cpf = input.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
        target.val(cpf);
    })

    $("#btnIncluirBeneficiario")
        .on('click', () => {

            let cpf = $("#beneficiarioCPF").val();
            let nome = $("#beneficiarioNome").val();
            let identificador = $("#identificadorBenfeficiario").val();

            if (cpf == "" || nome == "")
                return alert("Os campos CPF e Nome são obrigatórios!");
         
            if (identificador == "") {

                if (cpfRepetido(cpf))
                    return alert("Já existe uma beneficiário com esse CPF!");

                //adição
                let id = novoId();
                let linha = $('<tr>');
                linha.attr("id", id);

                //Coluna de CPF
                linha.append($('<td>').text(cpf));
                //Coluna de NOME
                linha.append($('<td>').text(nome));
                //Coluna de ações
                let btnEditar = $('<button>')
                    .addClass('btn btn-default')
                    .attr("type", "button")
                    .attr('onclick', 'alterarBeneficiario(this)')
                    .append($("<i>").addClass('glyphicon glyphicon-pencil'));

                let btnExcluir = $('<button>')
                    .addClass('btn btn-default')
                    .attr("type", "button")
                    .attr('onclick', 'excluirBeneficiario(this)')
                    .append($("<i>").addClass('glyphicon glyphicon-trash'));

                let colunaAcoes = $('<td>')
                    .append(btnEditar)
                    .append(btnExcluir);

                linha.append(colunaAcoes);

                beneficiarios.push({ Id: id, CPF: cpf, Nome: nome, isSaved: false, changed: false, isDeleted: false })
                $("#tblBeneficiarios tbody").append(linha);

            } else {
                //edição
                let linhas = $("#tblBeneficiarios tbody tr");

                for (let i = 0; i < linhas.length; i++) {
                    let linha = $(linhas[i]);
                    let idLinhaAtual = linha.attr("id");

                    if (idLinhaAtual == identificador) {
                        linha[0].cells[0].innerText = cpf;
                        linha[0].cells[1].innerText = nome;
                        break;
                    }
                }

                let obj = beneficiarios.find(x => x.Id == identificador);
                obj.CPF = cpf;
                obj.Nome = nome;
                obj.changed = true;
            }

            limparCampos();
        });
})

function novoId() {
    let novoID = 0;
    let maiorID = 0;
    
    for (let i = 0; i < beneficiarios.length; i++) {
        let id = beneficiarios[i].Id
        if (id > maiorID) {
            maiorID = id;
        }
    }

    novoID += maiorID + 1;
    return novoID;
}

function alterarBeneficiario(btn) {
    let id = $(btn)
        .parents("tr")
        .attr("id");

    let obj = beneficiarios.find(x => x.Id == id);

    if (obj == null || obj == undefined) {
        alert("Objeto não encontrado!");
        return;
    }

    $("#identificadorBenfeficiario").val(id);
    $("#beneficiarioCPF").val(obj.CPF);
    $("#beneficiarioNome").val(obj.Nome);
}

function excluirBeneficiario(btn) {
    let linha = $(btn)
        .parents("tr");

    let id = linha
        .attr("id");

    let beneficiario = beneficiarios.find(x => x.Id == id);

    if (beneficiario != null & beneficiario != undefined)
        beneficiario.isDeleted = true;

    linha.remove();
}

function buscarIndice(id) {
    for (let i = 0; i < beneficiarios.length; i++) {
        if (beneficiarios[i].Id == id) {
            return i;
        }
    }
}

function cpfRepetido(cpf) {
    let cpfs = beneficiarios.filter(x => x.CPF == cpf && x.isDeleted == false);
    return cpfs.length > 0 ? true : false;
}

function limparCampos() {
    $("#identificadorBenfeficiario").val("");
    $("#beneficiarioCPF").val("");
    $("#beneficiarioNome").val("");
}

function limparTabelaBeneficiarios() {
    beneficiarios = [];
    $("#tblBeneficiarios tbody").empty()
}
