//筆記右鍵選單顯示事件->筆記版本紀錄查詢事件 呼叫
function fn筆記版本紀錄查詢(paras) {
    $.ajax({
        url: paras.url_action,
        type: "POST",
        data: { FolderId: paras.FolderId, NoteId: paras.NoteId },
        beforeSend: function () {
            $("#loading-image").show();
        },
        success: function (msg) {
            console.log("版本控制查詢成功！");
            $(".lsVersion").html(msg);
            $("#loading-image").hide();
        }
    });
}
//資料夾右鍵選單顯示事件->資料夾右鍵選單重新命名事件 呼叫
function fn資料夾重新命名(paras){
    $.ajax({
        url: paras.url_action,
        type: "POST",
        data: {FolderId: paras.folderid, FolderName: paras.foldername},
        success: function(msg){
            console.log(msg);
        }
    });
}
//資料夾右鍵選單顯示事件->資料夾永久刪除事件 呼叫
function fn資料夾永久刪除(paras){
    console.log("資料夾永久刪除");
    console.log("paras.url_deletefolder = " + paras.url_deletefolder);
    console.log("paras.folderid = " + paras.folderid);
    $.ajax({
        url: paras.url_deletefolder,
        type: "POST",
        dataType: "html",
        data: {FolderId: paras.folderid},
        success: function(msg){
            console.log("msg = " + msg);
            $("#trash_block").html(msg);

            paras.selector.siblings("li").each(function( index ) {
                $(this).remove();
            });

            //新增筆記區的筆記資料夾清單一起移除
            let thisfolderid = paras.selector.data("folderid");
            $("#selFolder").find('option[data-folderid=' + thisfolderid+']').remove();
            paras.selector.parent("ul").remove();
            paras.selector.prev("i").remove();
            paras.selector.remove();
        }
    });
}
//Index新增筆記資料夾呼叫
function fn筆記資料夾新增(paras) {
    console.log("筆記資料夾新增");
    console.log("txtNewFolderName=" + paras.txtNewFolderName);

    $.ajax({
        url: paras.url_action,
        type: "POST",
        dataType: "html",
        data: { TxtNewFolderName: paras.txtNewFolderName },
        success: function (msg) {
            let notefolderid = msg;
            console.log("新增筆記資料夾的ID = " + notefolderid);

            $(".lsNote").append(`<ul class="connectedSortable" style="cursor: pointer;">` + `<i class='fas fa-caret-right fs-20'></i><a class='folder_list fs-20' id='folder_list_id_${notefolderid}' data-folderid='${notefolderid}' oncontextmenu='return false;'> ${paras.txtNewFolderName}</a></ul>`);
            $("#selFolder").append("<option data-folderid=" + notefolderid + ">" + paras.txtNewFolderName + "</option>");
        }
    });
}

//Index新增筆記呼叫
function fn筆記新增(paras) {
    console.log("function fn筆記新增(folderid, txtNewNoteName) {");
    console.log("folderid=" + paras.folderid);
    console.log("txtNewNoteName=" + paras.txtNewNoteName);

    $.ajax({
        url: paras.new_url_action,
        type: "POST",
        dataType: "html",
        data: { Folderid: paras.folderid, TxtNewNoteName: paras.txtNewNoteName },
        success: function (msg) {
            let noteid = msg;
            console.log("新增筆記的ID = " + noteid);
            let folder_list_id = "#folder_list_id_" + paras.folderid;

            $(folder_list_id).nextAll("li").show();
            $(folder_list_id).parent().append("<li class='note_list p-l-20 m-tb-5' data-noteid='" + noteid + "' style='display:list-item' oncontextmenu='return false;'><a class='fs-17 m-l-20'>" + paras.txtNewNoteName + "</a></li>");

            let icon = $(folder_list_id).prev(".fas");
            icon.removeClass("fa-caret-right");
            icon.addClass("fa-caret-down");
        }
    });
}
//筆記右鍵選單顯示事件->筆記右鍵選單我的最愛事件 呼叫
function fn筆記更新我的最愛(paras) {
    console.log("筆記更新我的最愛");
    console.log("noteId=" + paras.noteId);
    console.log("IsFavor=" + paras.isFavor);

    $.ajax({
        url: paras.url_action,
        type: "POST",
        dataType: "html",
        data: { FolderId: paras.folderid, NoteId: paras.noteId, IsFavor: paras.isFavor },
        success: function (msg) {
            console.log(msg);
        }
    });
}
//筆記右鍵選單顯示事件->筆記右鍵選單重新命名事件 呼叫
function fn筆記重新命名(paras) {
    console.log("筆記重新命名");
    console.log("noteId=" + paras.noteId);
    console.log("noteName=" + paras.noteName);

    $.ajax({
        url: paras.url_action,
        type: "POST",
        dataType: "html",
        data: { FolderId: paras.folderid, NoteId: paras.noteId, NoteName: paras.noteName },
        success: function (msg) {
            console.log(msg);
        }
    });
}
//Index quill 筆記文字改變事件 呼叫
function fn筆記儲存(url_action) {
    let FolderId = $(".select_note").prevAll(".folder_list").data("folderid");
    let NoteId = $(".select_note").data("noteid");
    if (FolderId === undefined || NoteId === undefined) return;

    console.log("筆記儲存");
    var content = JSON.stringify(quill.getContents());
    $.ajax({
        url: url_action,
        type: "POST",
        dataType: "html",
        data: { FolderId: FolderId, NoteId: NoteId, JsonContent: content },
        success: function (msg) {
            console.log(msg);
        }
    });
}
//筆記查詢事件 呼叫
function fn筆記查詢(paras) {
    console.log("筆記查詢");
    $.ajax({
        url: paras.url_action,
        type: "POST",
        dataType: "html",
        data: { FolderId: paras.FolderId, NoteId: paras.NoteId },
        success: function (msg) {
            console.log(JSON.parse(msg));
            quill.setContents("");
            quill.setContents(JSON.parse(msg));
        }
    });
}


//筆記右鍵選單顯示事件->筆記右鍵選單刪除事件 呼叫
function fn筆記移到垃圾桶(paras) {
    console.log("筆記移到垃圾桶");
    console.log("noteId=" + paras.noteId);
    let folderid = paras.selector.prevAll(".folder_list").data("folderid");
    $.ajax({
        url: paras.url_action_movetotrash,
        type: "POST",
        dataType: "html",
        data: { FolderId: folderid, NoteId: paras.noteId },
        success: function (msg) {
            $("#trash_block").html(msg);
            console.log(msg);
            paras.selector.remove();

            $(".alert-note-deleted").show("slow", function () {
                $(this).show("fast");
                $(this).fadeOut(3000);
            });
        }
    });
}
//筆記還原按鍵事件 呼叫
function fn筆記還原(paras) {
    console.log("筆記還原");
    console.log("noteId=" + paras.noteId);
    console.log("txtNoteName=" + paras.txtNoteName);
    $.ajax({
        url: paras.url_action,
        type: "POST",
        dataType: "html",
        data: { FolderId: paras.folderid, NoteId: paras.noteId },
        success: function (msg) {
            console.log(msg);
            //未分類筆記資料夾ID
            let folderid = $(".folder_list").filter(function () {
                return this.innerHTML === '未分類筆記';
            }).data("folderid");
            let folder_list_id = "#folder_list_id_" + folderid;
            $(folder_list_id).nextAll("li").show(); 
            //還原筆記到未分類資料夾
            $(folder_list_id).parent().append("<li class='note_list p-l-20 m-tb-5' data-noteid='" + paras.noteId + "' style='display:list-item'  oncontextmenu='return false;'><a>" + paras.txtNoteName + "</a></li>");

            let icon = $(folder_list_id).prev(".fas");
            icon.removeClass("fa-caret-right");
            icon.addClass("fa-caret-down");

            $(".alert-note-restore").show("slow", function () {
                $(this).show("fast");
                $(this).fadeOut(3000);
            });
        }
    });
}

//筆記永久刪除按鍵事件 呼叫
function fn筆記永久刪除(paras) {
    console.log("筆記永久刪除");
    console.log("noteId=" + paras.noteId);

    $.ajax({
        url: paras.url_action,
        type: "POST",
        dataType: "html",
        data: { FolderId: paras.folderid, NoteId: paras.noteId },
        success: function (msg) {
            $("#trash_block").html(msg);
            console.log(msg);
        }
    });
}
