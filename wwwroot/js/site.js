// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$(document).ready(
    function () {
        let token = GetVerificationToken();

        $("button.delete-button").click(function () {
            let itemId = $(this).data("id");
            let tr = $(this).parents("tr");
            $.post(window.location.href, { id: itemId, __RequestVerificationToken: token }, (result) => {
                if (result == "Ok") {
                    tr.hide(500);
                } else {
                    window.alert(result);
                }
            });
        });

        $("button#addStudentButton").click(function () {
            let parent = $("#addStudentModalCenter");
            let classroomId = parent.find("#ClassroomId").val();
            let newStudentId = parent.find("#NewStudentId").val();
            let url = window.location.href.includes('?') ? window.location.href + "&handler=AddStudent" : window.location.href + "?handler=AddStudent";
            $.post(url, { classroomId: classroomId, userId: newStudentId, __RequestVerificationToken: token }, (result) => {
                if (result == "Ok") {
                    parent.modal("toggle");
                } else {
                    window.alert(result);
                }
            });
        });

        $("button.classroom-edit-button").click(function () {
            let tr = $(this).parents("tr");
            let editor = tr.find(".classroom-editor")[0];
            let link = tr.find(".classroom-editor-display")[0];
            let button = tr.find(".classroom-edit-save-button")[0]
            $(editor).toggleClass("d-none");
            $(link).toggleClass("d-none");
            $(button).toggleClass("d-none");
            $(this).toggleClass("d-none");
        });

        $("button.classroom-edit-save-button").click(function () {
            let tr = $(this).parents("tr");
            let editor = tr.find(".classroom-editor")[0];
            let link = tr.find(".classroom-editor-display")[0];
            let button = tr.find(".classroom-edit-button")[0]
            $(editor).toggleClass("d-none");
            $(link).toggleClass("d-none");
            $(button).toggleClass("d-none");
            $(this).toggleClass("d-none");
            let itemId = $(this).data("id");
            let url = window.location.href.includes('?') ? window.location.href + "&handler=Rename" : window.location.href + "?handler=Rename";
            $.post(url, { id: itemId, newName: $(editor).val(), __RequestVerificationToken: token }, (result) => {
                if (result == "Ok") {
                    $(link).text($(editor).val().trim());
                } else {
                    window.alert(result);
                }
            });
        });

        function resetOptionIndexes() {
            let options = $("#SelectionOptions tbody tr");
            for (let i = 0; i < options.length; i++) {
                let elements = $(options[i]).find("[name^='QuestionModel.SelectionAnswers']");
                for (let e of elements) {
                    let name = $(e).attr("name");
                    let newName = name.replace(/QuestionModel\.SelectionAnswers\[\d+\]\.(.*)/gi, (match, p1) => `QuestionModel.SelectionAnswers[${i}].${p1}`);
                    $(e).attr("name", newName);
                }
            }
        }

        $(document).on("click", "button.delete-option-button", function () {
            $(this).parents("tr").remove();
            resetOptionIndexes();
        });

        $("button.add-option-button").click(function () {
            $(this).parents("tr").before(`
<tr>
	<td class="align-middle">
		<input class="form-control" type="hidden" data-val="true" name="QuestionModel.SelectionAnswers[0].Id" value="0">
		<input class="form-control" type="hidden" data-val="true" name="QuestionModel.SelectionAnswers[0].QuestionId" value="0">
		<input class="form-control" type="text" data-val="true" data-val-required="The Content field is required." name="QuestionModel.SelectionAnswers[0].Content" value="" placeholder="请输入答案描述">
		<span class="text-danger field-validation-valid" data-valmsg-for="QuestionModel.SelectionAnswers[0].Content" data-valmsg-replace="true"></span>
	</td>
	<td class="align-middle">
		<select class="form-control" data-val="true" data-val-required="The Correct field is required." name="QuestionModel.SelectionAnswers[0].Correct">
			<option value="True">是</option>
			<option value="False" selected="selected">否</option>
		</select>
	</td>
	<td class="align-middle"><button type="button" class="btn btn-danger delete-option-button">删除</button></td>
</tr>`);
            resetOptionIndexes();
        });
    }
);


function GetVerificationToken() {
    let element = $("[name='__RequestVerificationToken']");
    if (element === undefined) {
        return null;
    }

    let token = element.val();
    if (token === undefined) {
        return null;
    }

    return token;
}