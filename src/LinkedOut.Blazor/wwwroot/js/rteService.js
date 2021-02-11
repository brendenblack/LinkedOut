
function initializeRichTextEditor(id) {
    console.log(`Initializing RichTextEditor for element ${id}`);
    var config = {};
    config.toolbar = "basic";
    config.editorResizeMode = "height";
    config.showFloatParagraph = false;
    
    // console.log('RTE configuration', config);
    
    var editor = new RichTextEditor(id, config);

    console.log('Reference created', editor);
    return editor;
}

/**
 * Returns the contents in the editor as an HTML string. The editor must have 
 * first been initialized by calling initializeRichTextEditor
 **/
function getEditorContentAsHtml(editor) {
    // console.log('Getting editor content');
    console.log(editor);
    return editor.getHTMLCode();
}

function setEditorContent(editor, contents) {
    // console.log('Setting editor contents', contents);
    editor.setHTMLCode(contents);
}

function registerCallback(editor, dotnetobject) {
    console.log('Registering callback to dotnet', dotnetobject);

    editor.attachEvent("change", function () {
        var html = editor.getHTMLCode();
        // console.log('Invoking dotnet method');
        dotnetobject.invokeMethodAsync('UpdateContents', html);
    })
}
