// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$(document).ready(
    function () {
        let token = GetVerificationToken();
        //$("button[data-userid]").click(function () {
        //    $(this).parents("tr").hide(500);
        //    let userId = $(this).data("userid");
        //    $.post(window.location, { id: userId, __RequestVerificationToken: token });
        //});

        //$("button[data-chapterid]").click(function () {
        //    $(this).parents("tr").hide(500);
        //    let userId = $(this).data("chapterid");
        //    $.post(window.location, { id: userId, __RequestVerificationToken: token });
        //});

        $("button.delete-button").click(function () {
            $(this).parents("tr").hide(500);
            let itemId = $(this).data("id");
            $.post(window.location, { id: itemId, __RequestVerificationToken: token });
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
</tr>`)
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