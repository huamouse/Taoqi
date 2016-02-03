

//创建上传适配器
//////diplayID:显示图片名称
/////PickerID :选择器
////NewPictureID:显示新的图片ID
/// SaveFiled:保存的字段名成
function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (prePath + postPath);
}

function CreateUploader(PickerID, NewPictureID, tableName, tableFiled, ImageUp) {
    var $ = jQuery,
   // $list = $(diplayID),//显示名称
    $NewPicture = $(NewPictureID),
     $ImageUp = $(ImageUp),
     $PickerID = $(PickerID),
    // 优化retina, 在retina下这个值是2
    ratio = window.devicePixelRatio || 1,

    // 缩略图大小
    thumbnailWidth = 100 * ratio,
    thumbnailHeight = 100 * ratio,
    // Web Uploader实例
    uploader;
    var ee = window.getQueryString("ID");
    // 初始化Web Uploader
    uploader = WebUploader.create({
        // 自动上传。
        auto: true,
        // swf文件路径
        swf: getRootPath() + '/Include/javascript/Upload/Uploader.swf',
        // 文件接收服务端。
        server: getRootPath() + '/Include/javascript/Upload/Upload.aspx',
        formData: {
            tbName: tableName,
            tbFields: tableFiled,
            tbId: ee
        },
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: PickerID,
        fileNumLimit: 1,
        //duplicate: true,
        // 如果发现压缩后文件大小比原来还大，则使用原来图片
        // 此属性可能会影响图片自动纠正功能
        noCompressIfLarger: false,

        //// 单位字节，如果图片大小小于此值，不会采用压缩。
        //compressSize: 1024 * 1024 * 100,
        ////不压缩
        resize: false,
        // 只允许选择文件，可选。
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        }
    });
    SetWebUploaderEvent(ratio, thumbnailWidth, thumbnailHeight, uploader, $NewPicture, $PickerID, $ImageUp);
}
//配置上传图片事件
function SetWebUploaderEvent(ratio, thumbnailWidth, thumbnailHeight, uploader, $NewPicture, $PickerID, $ImageUp) {
    uploader.on('beforeFileQueued', function (file) {
        uploader.reset();
    });

    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {
        //var $li = $(
        //        '<div id="' + file.id + '" class="file-item">' +
        //            '<img>' +
        //            '<div class="info">' + file.name + '</div>' +
        //        '</div>'
        //        ),
        //    $img = $li.find('img');
        //$list.empty();
        //$list.append($li);

        // 创建缩略图
        uploader.makeThumb(file, function (error, src) {
            if (error) {
                $img.replaceWith('<span>不能预览</span>');
                $ImageUp.replaceWith('<span>不能预览</span>');
                return;
            }
            //$img.attr('src', src);
            $ImageUp.attr('src', src);
            $ImageUp.attr('alt', file.id);
        }, thumbnailWidth, thumbnailHeight);

    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress span');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<p class="progress"><span></span></p>')
                    .appendTo($li)
                    .find('span');
        }
        $percent.css('width', percentage * 100 + '%');
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    //这里可加 Data，来记住生成后的名称
    uploader.on('uploadSuccess', function (file, data) {
        $('#' + file.id).addClass('upload-state-done');
        ///设置新生成的ID
        $NewPicture.val(data.id);
        //  alert(data.id);
    });

    // 文件上传失败，现实上传出错。
    uploader.on('uploadError', function (file) {
        var $li = $('#' + file.id),
            $error = $li.find('div.error');

        // 避免重复创建
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }

        $error.text('上传失败');
    });

    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
        console.log(getRootPath() + '/Include/javascript/Upload/Upload.aspx');
    });
    //文件

    // 先从文件队列中移除之前上传的图片，第一次上传则跳过
    $PickerID.on('click', function () {
        //if (!WebUploader.Uploader.support()) {
        //    var error = "上传控件不支持您的浏览器！请尝试升级flash版本或者使用Chrome引擎的浏览器。<a target='_blank' href='http://se.360.cn'>下载页面</a>";
        //    layer.alert('fadsfas');
        //    console.log(error);
        //    return;
        //}

        // var id = $list.find("div").attr("id");
        var id = $ImageUp.attr("alt");
        if (undefined != id) {
            uploader.removeFile(uploader.getFile(id));
        }
    });
}