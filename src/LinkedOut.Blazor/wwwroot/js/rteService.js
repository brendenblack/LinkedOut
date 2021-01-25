
function initializeRichTextEditor(id) {
    console.log(`Initializing RichTextEditor for element ${id}`);
    var editor1cfg = {}
    editor1cfg.toolbar = "basic";   
    var editor = new RichTextEditor(id, editor1cfg); 
    console.log('Reference created', editor);
    return editor;
}

/**
 * Returns the contents in the editor as an HTML string. The editor must have 
 * first been initialized by calling initializeRichTextEditor
 **/
function getEditorContentAsHtml(editor) {
    console.log('Getting editor content');
    console.log(editor);
    return editor.getHTMLCode();
}