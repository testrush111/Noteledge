//筆記右鍵選單預設關閉
var isNoteContextMenuClose = true;
//資料夾右鍵選單預設關閉
var isFolderContextMenuClose = true;

function fn資料夾右鍵選單關閉事件() {
    $(window).click(function () {
        //如果資料夾右鍵選單開啟
        if (isFolderContextMenuClose === false) {
            ////資料夾右鍵選單關閉
            $("#folder_menu").hide();
        }
    });
}

function fn資料夾右鍵選單顯示事件(paras) {
    console.log("資料夾右鍵選單顯示事件");
    //預設資料夾ID
    let folderid = $(".folder_list").filter(function () {
        return this.innerHTML === '未分類筆記';
    }).data("folderid");
    //所有預設或動態新增的資料夾註冊事件
    $(".lsNote").delegate(`.folder_list[data-folderid!='${folderid}']`, "mousedown", (function (e) {
        //滑鼠右鍵
        if (e.which === 3) {
            //顯示資料夾選單
            isFolderContextMenuClose = false;
            //筆記右鍵選單顯示就關閉它
            if (isNoteContextMenuClose === false) {
                console.log("取消右鍵點選");
                $("#note_menu").hide();
            }
            console.log("資料夾右鍵點選");
            //設定選單位置
            $("#folder_menu").css({
                left: e.pageX + "px",
                top: e.pageY + "px"
            });
            //選單顯示
            $("#folder_menu").show();
            fn資料夾右鍵選單重新命名事件({
                url_action: paras.url_folderrename,
                selector: $(this)
            });
            fn資料夾永久刪除事件({
                url_action_movetotrash: paras.url_movetotrash,
                url_moveouttrash: paras.url_moveoutrash,
                url_deletenote: paras.url_deletenote,
                url_deletefolder: paras.url_deletefolder,
                url_imgundo: paras.url_imgundo,
                url_imgtrash: paras.url_imgtrash,
                selector: $(this)
            });
        }
    }));
}

function fn筆記右鍵選單關閉事件() {
    $(window).on("click", (function () {
        //如果筆記右鍵選單開啟時
        if (isNoteContextMenuClose === false) {
            console.log("取消右鍵點選");
            //筆記右鍵選單關閉
             $("#note_menu").hide();
        }
    }));
}
//--------------------------------------------------------------------
//============================資料夾右鍵選單重新命名事件============================
//--------------------------------------------------------------------
function fn資料夾右鍵選單重新命名事件(paras) {
    $("#rename_folder").off("click");
    $("#rename_folder").one("click", function () {
        //允許編輯
        paras.selector.attr("contenteditable", "true");
        paras.selector.css("background-color", "#D6D6FF");
        
        paras.selector.focus();
        //**********離開焦點**************************
        paras.selector.blur(function () {
            var noteName = $(this).html();//取得重新命名的筆記名字
            console.log("取得重新命名的資料夾名字：" + noteName);
            //不允許編輯
            $(this).attr("contenteditable", "false");
            $(this).css("background-color", "white");
            //資料夾ID
            let folderid = $(this).data("folderid");
            //資料夾名稱
            let foldername = $(this).html();
            console.log("fn資料夾右鍵選單重新命名事件 folderid = " + folderid);
            console.log("fn資料夾右鍵選單重新命名事件 foldername = " + foldername);
            fn資料夾重新命名({
                url_action: paras.url_action,
                folderid: folderid,
                foldername: foldername
            });
        });
    });
}
//--------------------------------------------------------------------
//============================資料夾永久刪除事件============================
//--------------------------------------------------------------------
function fn資料夾永久刪除事件(paras) {
    $("#delete_folder").off("click");
    $("#delete_folder").one("click", function () {
        console.log("fn資料夾永久刪除事件");
        console.log("paras.url_deletefolder = " + paras.url_deletefolder);
        //資料夾ID
        let folderid = $(paras.selector).data("folderid");
        console.log("folderid = " + folderid);
        const swal = Swal.mixin();

        swal.fire({
            title: '確認刪除筆記資料夾?',
            text: "永久刪除後，筆記資料夾就回不來了喔~~~",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '確認',
            cancelButtonText: '取消',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire(
                    'Deleted!',
                    '筆記資料夾已經永久刪除(筆記已移至垃圾桶)。',
                    'success'
                );
                fn資料夾永久刪除({
                    url_deletefolder: paras.url_deletefolder,
                    url_moveouttrash: paras.url_moveouttrash,
                    url_deletenote: paras.url_deletenote,
                    url_imgundo: paras.url_imgundo,
                    url_imgtrash: paras.url_imgtrash,
                    selector: paras.selector,
                    folderid: folderid
                });
            }
        });
        //if (confirm("永久刪除後，資料就回不來了喔~~~")) {
        //    fn資料夾永久刪除({
        //        url_deletefolder: paras.url_deletefolder,
        //        url_moveouttrash: paras.url_moveouttrash,
        //        url_deletenote: paras.url_deletenote,
        //        url_imgundo: paras.url_imgundo,
        //        url_imgtrash: paras.url_imgtrash,
        //        selector: paras.selector,
        //        folderid: folderid
        //    });
        //}
    });
}

function fn筆記右鍵選單顯示事件(paras) {
    console.log("筆記右鍵選單顯示 事件註冊!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    //所有預設或動態新增的筆記註冊事件
    $(".lsNote").delegate(paras.selector, "mousedown", (function (e) {
        //右鍵點選
        if (e.which === 3) {
            //筆記選單開啟
            isNoteContextMenuClose = false;
            //資料夾右鍵選單開啟就關閉它
            if (isFolderContextMenuClose === false) {
                console.log("取消右鍵點選");
                $("#folder_menu").hide();
            }
            
            //$(".note_list").not(this).removeClass("context_click");
            //$(this).addClass("context_click");

            console.log("筆記右鍵點選");
            //設定選單位置
            $("#note_menu").css({
                left: e.pageX + "px",
                top: e.pageY + "px"
            });
            //如果有星星標誌則顯示移除我的最愛
            if ($(this).children().is(".fa-star")) {
                $("#favor_note").html(`<i class='fas fa-folder-minus' style='font-size:20px;vertical-align: middle;'></i> 移除我的最愛`);
            }
            //沒有則顯示+我的最愛
            else {
                $("#favor_note").html(`<span class="material-icons" style="vertical-align: middle;">folder_special</span> 加入我的最愛`);
            }
            //筆記右鍵選單顯示
            $("#note_menu").show();


            fn筆記右鍵選單我的最愛事件({
                url_action: paras.url_updatefavor,
                url_imgstarred: paras.url_imgstarred,
                selector: $(this)
            });
            fn筆記右鍵選單刪除事件({
                url_action_movetotrash: paras.url_movetotrash,
                url_moveouttrash: paras.url_moveoutrash,
                url_deletenote: paras.url_deletenote,
                url_imgundo: paras.url_imgundo,
                url_imgtrash: paras.url_imgtrash,
                selector: $(this)
            });
            fn筆記右鍵選單重新命名事件({
                url_action: paras.url_noterename,
                selector: $(this)
            });
            fn筆記版本紀錄查詢事件({
                url_action: paras.url_searchversion,
                selector: $(this)
            });
        }
    }));
}

function fn筆記版本紀錄查詢事件(paras) {
    $("#version_note").off("click");
    $("#version_note").one("click", function () {
        //版本紀錄區塊顯示
        $("#version_block").show();
        console.log("click筆記版本紀錄查詢");
        //筆記ID
        let noteid = $(paras.selector).data("noteid");
        //資料夾ID
        let folderid = paras.selector.prevAll(".folder_list").data("folderid");
        
        fn筆記版本紀錄查詢({
            url_action: paras.url_action,
            FolderId: folderid,
            NoteId: noteid,
        });
    });
}

function fn筆記還原按鍵事件(paras) {
    $("#trash_block").delegate(paras.selector, "click", function () {
        //還原的筆記從垃圾桶移除
        $(this).parent().remove();
        console.log("click筆記還原");
        //資料夾ID
        let folderid = $(this).parent().data("folderid");
        //筆記ID
        let noteid = $(this).parent().data("noteid");
        //筆記名稱
        let txtNoteName = $(this).parent().children("span").text();

        console.log($(this).text());
        //url_action, folderid, noteId
        fn筆記還原({
            url_action: paras.url_action,
            folderid: folderid,
            noteId: noteid,
            txtNoteName: txtNoteName
        });
    });
}

function fn筆記永久刪除按鍵事件(paras) {
    $("#trash_block").delegate(paras.selector, "click", function () {
        console.log("click筆記永久刪除");
        //資料夾ID
        let folderid = $(this).parent().data("folderid");
        //筆記ID
        let noteid = $(this).parent().data("noteid");

        const swal = Swal.mixin();

        swal.fire({
            title: '確認刪除?',
            text: "永久刪除後，資料就回不來了喔~~~",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '確認',
            cancelButtonText: '取消',
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire(
                    'Deleted!',
                    '筆記已經永久刪除。',
                    'success'
                );
                fn筆記永久刪除({
                    url_action: paras.url_action,
                    url_moveouttrash: paras.url_moveouttrash,
                    url_deletenote: paras.url_deletenote,
                    folderid: folderid,
                    noteId: noteid
                });
            }
        });
    });
}

function fn筆記右鍵選單我的最愛事件(paras) {
    //筆記ID
    let noteid = $(paras.selector).data("noteid");
    $("#favor_note").off("click");
    $("#favor_note").one("click", function () {
        //資料夾ID
        let folderid = paras.selector.prevAll(".folder_list").data("folderid");
        //移除我的最愛
        if (paras.selector.children().is(".fa-star")) {
            paras.selector.children(".fa-star").first().remove();
            paras.selector.children("a").removeClass("favor_note").addClass("m-l-20");
            fn筆記更新我的最愛(
                {
                    url_action: paras.url_action,
                    folderid: folderid,
                    noteId: noteid,
                    isFavor: false
                }
            );
        }
        //加到我的最愛
        else {
            $(this).html(`<span class="material-icons" style="vertical-align: middle;">folder_special</span> 加入我的最愛`);
            //paras.selector.prepend(`<img src="${paras.url_imgstarred}" />`);
            paras.selector.prepend(`<i class="fas fa-star text-warning"></i>`);
            paras.selector.children("a").addClass("favor_note").removeClass("m-l-20");
            fn筆記更新我的最愛(
                {
                    url_action: paras.url_action,
                    folderid: folderid,
                    noteId: noteid,
                    isFavor: true
                }
            );
        }

    });
}

function fn筆記右鍵選單刪除事件(paras) {
    $("#delete_note").off("click");
    $("#delete_note").one("click", function () {
        //筆記ID
        let noteid = $(paras.selector).data("noteid");
        //筆記名稱
        let notename = $(paras.selector).children("a").html();
        
        fn筆記移到垃圾桶({
            url_action_movetotrash: paras.url_action_movetotrash,
            url_moveouttrash: paras.url_moveouttrash,
            url_deletenote: paras.url_deletenote,
            url_imgundo: paras.url_imgundo,
            url_imgtrash: paras.url_imgtrash,
            selector: paras.selector,
            noteId: noteid,
            notename: notename
        });
    });
}

function fn筆記右鍵選單重新命名事件(paras) {
    $("#rename_note").off("click");
    $("#rename_note").one("click", function () {
        paras.selector.children("a").eq(0).attr("contenteditable", "true");
        paras.selector.children("a").focus();
        //**********離開焦點**************************
        paras.selector.children("a").eq(0).blur(function () {
            //取得重新命名的筆記名字
            var noteName = $(this).html();
            console.log("取得重新命名的筆記名字：" + noteName);

            $(this).attr("contenteditable", "false");

            let folderid = $(paras.selector).prevAll(".folder_list").data("folderid");
            let noteid = $(paras.selector).data("noteid");
            //url_action, folderid, noteId, noteName
            fn筆記重新命名({
                url_action: paras.url_action,
                folderid: folderid,
                noteId: noteid,
                noteName: noteName
            });
        });
    });
}

function fn筆記右鍵選單複製事件(selector) {
    $("#copy_note").one("click", function () {
    });
}

function fn筆記查詢事件(paras) {
    console.log("筆記查詢事件");
    $(".lsNote").delegate(paras.selector, "click", (function () {
        //資料夾ID
        let folderid = $(this).prevAll(".folder_list").data("folderid");
        //筆記ID
        let noteid = $(this).data("noteid");
        fn筆記查詢({
            url_action: paras.url_action,
            FolderId: folderid,
            NoteId: noteid
        });

        $(".note_list").not(this).removeClass("select_note");
        $(this).addClass("select_note");
    }));
}
