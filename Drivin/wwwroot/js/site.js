// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // Show notifications
    function toastNotification(level, message) {
        var toast = $(`
            <div class="toast align-items-center text-white border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        Hello, world! This is a toast message.
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        `);

        switch (level) {
            case "info":
                toast.addClass("bg-primary");
                break;
            case "success":
                toast.addClass("bg-success");
                break;
            case "error":
                toast.addClass("bg-danger");
                break;
            default:
                toast.addClass("bg-secondary");
                break;
        }
        toast.find(".toast-body").text(message);

        $(".toast-container").append(toast);
        toast.toast("show");

        // Remove the toast from DOM after it"s hidden
        toast.on("hidden.bs.toast", function () {
            $(toast).remove();
        });
    }

    // Manage page state
    function setFileUploadState(state, message) {
        switch (state) {
            case "idle":
                // hide loading
                $("#uploadButton").show();
                $("#loading").hide();
                $("#table").hide();

                // disable both buttons while file is not selected
                $("#uploadButton").prop("disabled", true);
                $("#cleanUploadButton").prop("disabled", true);
                break;

            case "waiting":
                // hide loading
                $("#uploadButton").show();
                $("#loading").hide();
                $("#table").hide();

                // enable both buttons after file is selected
                $("#uploadButton").prop("disabled", false);
                $("#cleanUploadButton").prop("disabled", false);

                // show feedback
                toastNotification("info", message ?? "Arquivo selecionado.");
                break;

            case "processing":
                // show loading
                $("#uploadButton").hide();
                $("#loading").show();
                $("#table").hide();

                // disable cancel button
                $("#cleanUploadButton").prop("disabled", true);

                // show feedback
                toastNotification("info", message ?? "Processando.");
                break;

            case "done":
                // hide loading
                $("#uploadButton").show();
                $("#loading").hide();
                $("#table").show();

                // disable both buttons while file is not selected
                $("#uploadButton").prop("disabled", true);
                $("#cleanUploadButton").prop("disabled", true);

                // show feedback
                toastNotification("success", message ?? "Processado com sucesso.");
                break;

            case "error":
                // hide loading
                $("#uploadButton").show();
                $("#loading").hide();
                $("#table").hide();

                // enable both buttons after file is selected
                $("#uploadButton").prop("disabled", false);
                $("#cleanUploadButton").prop("disabled", false);

                // show feedback
                toastNotification("error", message ?? "Erro ao carregar o arquivo.");
                break;

            default:
                throw new Error("Unknown State.")
        }
    }

    // Clean file input
    function cleanUploadInput() {
        $("#file").val("");
    }

    // Create table with the request response
    function createTableFromList(subfiles) {
        const $tableBody = $("#dynamicTable tbody");
        $tableBody.empty();

        subfiles.forEach(subfile => {
            const $row = $("<tr>");

            $row.append($("<td>")
                .text(subfile.fileName));

            const $emails = $("<div>");
            subfile.emails.forEach(email => $emails.append($("<div>").text(email)));
            $row.append($("<td>").append($emails));

            const $link = $("<a>")
                .attr("href", subfile.url)
                .text("Download");
            $row.append($("<td>").append($link));

            $tableBody.append($row);
        });
    }

    // Select a file
    $("#file").change(function (e) {
        if ($(this).val()) {
            setFileUploadState("waiting");
        }
    });

    // Click on cancel button
    $("#cleanUploadButton").click(function () {
        cleanUploadInput();
        setFileUploadState("idle");
    });

    // Click on send file button
    $("#uploadButton").click(function () {
        console.warn("$('#uploadButton').click")

        const form = $("#uploadForm").get(0);
        var formData = new FormData(form);

        $.ajax({
            url: "/EmailsFile",
            type: "POST",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: (b) => {
                // FIXME: remover logs
                console.log({ b })
                setFileUploadState("processing", "Por favor, aguarde...");
            },
            success: (s) => {
                // FIXME: remover logs
                console.log({ s })
                cleanUploadInput();
                setFileUploadState("done", "Arquivo processado com sucesso.");
                createTableFromList(s)
            },
            error: (e) => {
                // FIXME: remover logs
                console.log({ e })
                setFileUploadState("error", e.responseText);
            }
        });
    });
});
