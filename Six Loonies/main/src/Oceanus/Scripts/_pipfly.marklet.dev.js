var oceanusDevMode = false;
var oceanusPostCSSClassName = "oceanus-content";
var showingArticleClip = false;
var showingPhotoClip = false;
var showingVideoClip = false;

var debug = function (s) {
    if (typeof console !== 'undefined' && oceanusDevMode)
        console.log("Oceanus: " + s);
};

var oceanus = {
    version: '0.1',
    iframeLoads: 0,
    frameHack: false, /**
	                     * The frame hack is to workaround a firefox bug where if you
						 * pull content out of a frame and stick it into the parent element, the scrollbar won't appear.
						 * So we fake a scrollbar in the wrapping div.
						**/
    documentCopy: null,   /* Copy of the document body */

    /**
    * All of the regular expressions in use within oceanus.
    * Defined up here so we don't instantiate them repeatedly in loops.
    **/
    regexps: {
        unlikelyCandidatesRe: /combx|comment|disqus|foot|header|menu|meta|navmenu|navigation|dropdown|\bnav\b|rss|shoutbox|sidebar|sponsor/i,
        unlikelyAdLinksRe: /offercode/i,
        maybeCandidateRe: /and|article|body|column|main/i,
        positiveRe: /article|body|content|entry|hentry|page|pagination|post|text/i,
        negativeRe: /combx|comment|contact|foot|footer|footnote|link|media|meta|promo|related|scroll|shoutbox|sponsor|tags|widget/i,
        divToPElementsRe: /<(a|blockquote|dl|div|img|ol|p|pre|table|ul)/i,
        divToPElementsWithLinkRe: /<(blockquote|dl|div|img|ol|p|pre|table|ul)/i,
        replaceBrsRe: /(<br[^>]*>[ \n\r\t]*){2,}/gi,
        replaceFontsRe: /<(\/?)font[^>]*>/gi,
        postRe: /(\bpost\b)|(\bpost-([1234567890]{1,})\b)|(\bcnnPostWrap\b)|(\bpostbox\b)|(\bstory\b)/i,
        trimRe: /^\s+|\s+$/g,
        normalizeRe: /\s{2,}/g,
        killBreaksRe: /(<br\s*\/?>(\s|&nbsp;?)*){1,}/g,
        videoRe: /http:\/\/(www\.)?(youtube|vimeo)\.com/i,
        closeTextRe: /close/i
    },

    /**
    * Runs oceanus.
    * 
    * Workflow:
    *  1. Prep the document by removing script tags, css, etc.
    *  2. Build oceanus's DOM tree.
    *  3. Grab the article content from the current dom tree.
    *  4. Replace the current DOM tree with the new one.
    *  5. Read like a newspaper.
    *
    * @return void
    **/
    init: function (preserveUnlikelyCandidates) {

        if (document.body && !oceanus.documentCopy) {
            oceanus.documentCopy = document.createElement("body");
            oceanus.documentCopy.innerHTML = document.body.innerHTML;
        }

        oceanus.prepDocument();

        /* Build oceanus's DOM tree */
        var overlay = document.createElement("DIV");
        var innerDiv = document.createElement("DIV");
        var articles = oceanus.grabArticles();

        /**
        * If we attempted to strip unlikely candidates on the first run through, and we ended up with no content,
        * that may mean we stripped out the actual content so we couldn't parse it. So re-run init while preserving
        * unlikely candidates to have a better shot at getting our content out properly.
        **/
        if (articles.length < 1) {
            //alert("Sorry, pipfly was unable to parse this page for content. If you feel like it should have been able to, please <a href='http://www.pipfly.com'>let us know by submitting an issue.</a>");
            return;
        }
        else {
            for (var i = 0; i < articles.length; ++i) {
                var articleTitleText = "";
                try {
                    var articleTitleElem = sl_jQuery('.oceanus-title', articles[i]);
                    if (articleTitleElem != 'undefined' && articleTitleElem != null && articleTitleElem.length > 0) {
                        articleTitleElem = articleTitleElem[0];
                        var innerText = oceanus.getInnerText(articleTitleElem, false);
                        articleTitleText = sl_jQuery.trim(oceanus.getInnerText(articleTitleElem, false));
                    }
                }
                catch (e) {
                }
                if ((oceanus.getInnerText(articles[i], false).length - articleTitleText.length) < 275) {
                    articles.splice(i, 1);
                    --i;
                }
            }

            if (articles.length >= 1) {
                document.articles = articles;
            }
            else {
                document.articles = null;
            }
        }
    },

    /**
    * Get the article title as an H1. Currently just uses document.title,
    * we might want to be smarter in the future.
    *
    * @return void
    **/
    getArticleTitle: function () {
        var articleTitleLink = document.createElement("a");
        articleTitleLink.href = window.location.href;
        var articleTitle = document.createElement("h2");
        articleTitle.className = "oceanus-title";
        articleTitle.innerHTML = document.title;
        articleTitleLink.appendChild(articleTitle);

        return articleTitleLink;
    },

    /**
    * Prepare the HTML document for oceanus to scrape it.
    * This includes things like stripping javascript, CSS, and handling terrible markup.
    * 
    * @return void
    **/
    prepDocument: function () {
        /**
        * In some cases a body element can't be found (if the HTML is totally hosed for example)
        * so we create a new body node and append it to the document.
        */
        if (document.body === null) {
            body = document.createElement("body");
            try {
                oceanus.documentCopy = body;
            }
            catch (e) {
                document.documentElement.appendChild(body);
                oceanus.documentCopy = document.body;
            }
        }

        var frames = oceanus.documentCopy.getElementsByTagName('frame');
        if (frames.length > 0) {
            var contentFrame = null;
            var contentFrameSize = 0;
            for (var frameIndex = 0; frameIndex < frames.length; frameIndex++) {
                var frameSize = frames[frameIndex].offsetWidth + frames[frameIndex].offsetHeight;
                var canAccessFrame = false;
                try {
                    frames[frameIndex].contentWindow.document.body;
                    canAccessFrame = true;
                } catch (e) { }

                if (canAccessFrame && frameSize > contentFrameSize) {
                    contentFrame = frames[frameIndex];
                    contentFrameSize = frameSize;
                }
            }

            if (contentFrame) {
                var newBody = document.createElement('body');
                newBody.innerHTML = contentFrame.contentWindow.document.body.innerHTML;
                newBody.style.overflow = 'scroll';
                oceanus.documentCopy = newBody;
            }
        }

        /* remove all scripts that are not pipfly */
        var scripts = oceanus.documentCopy.getElementsByTagName('script');
        for (i = scripts.length - 1; i >= 0; i--) {
            if (typeof (scripts[i].src) == "undefined" || (scripts[i].src.indexOf('pipfly') == -1 || (oceanusDevMode && scripts[i].src.indexOf('localhost') == -1))) {
                scripts[i].parentNode.removeChild(scripts[i]);
            }
        }

        /* Remove all style tags in head */
        var styleTags = oceanus.documentCopy.getElementsByTagName("style");
        for (var j = 0; j < styleTags.length; j++) {
            if (navigator.appName != "Microsoft Internet Explorer") {
                styleTags[j].textContent = "";
            }
        }

        /* Remove all popup tags */
        var linkTags = oceanus.documentCopy.getElementsByTagName("a");
        for (var k = 0; k < linkTags.length; k++) {
            var linkText = oceanus.getInnerText(linkTags[k], true);

            if (linkText.search(oceanus.regexps.closeTextRe) == 0) {
                var parentNodeElem = linkTags[k].parentNode;
                var grandParentNodeElem = null;

                if (parentNodeElem != 'undefined' && parentNodeElem != null) {
                    grandParentNodeElem = parentNodeElem.parentNode;
                }

                if (grandParentNodeElem != 'undefined' && grandParentNodeElem != null) {
                    grandParentNodeElem.removeChild(parentNodeElem);
                }
            }
        }

        //compile reference to all hidden elements in the document
        var hiddenElems = new Array();
        for (var nodeIndex = 0; (node = document.getElementsByTagName('div')[nodeIndex]); nodeIndex++) {
            if ((node.id !== 'undefined' && node.id != null && node.id != '') || (node.className !== 'undefined' && node.className != null && node.className != '')) {
                if (isHidden(node)) {
                    hiddenElems.push(node);
                }
            }
        }
        var hiddenImgElems = new Array();
        for (var nodeIndex = 0; (node = document.getElementsByTagName('img')[nodeIndex]); nodeIndex++) {
            if ((node.id !== 'undefined' && node.id != null && node.id != '') || (node.className !== 'undefined' && node.className != null && node.className != '')) {
                if (isHidden(node)) {
                    hiddenImgElems.push(node);
                }
            }
        }

        //IMPORTANT! Determine all paragraphs contained in floating parents / grandparent nodes
        var floatingParagraphs = new Array();
        for (var nodeIndex = 0; (node = document.getElementsByTagName('p')[nodeIndex]); nodeIndex++) {
            var parseParentNode = node.parentNode;
            var parseGrandParentNode = null;

            if (parseParentNode != null) {
                parseGrandParentNode = parseParentNode.parentNode;
            }

            if (parseParentNode != null && sl_jQuery(parseParentNode).css('float') != 'undefined' && (sl_jQuery(parseParentNode).css('float') == 'right' || sl_jQuery(parseParentNode).css('float') == 'left')) {
                floatingParagraphs.push(node);
                continue;
            }

            if (parseGrandParentNode != null && sl_jQuery(parseGrandParentNode).css('float') != 'undefined' && (sl_jQuery(parseGrandParentNode).css('float') == 'right' || sl_jQuery(parseGrandParentNode).css('float') == 'left')) {
                floatingParagraphs.push(node);
                continue;
            }
        }

        document.floatingParagraphs = floatingParagraphs;

        //remove all hidden divs
        for (var nodeIndex = 0; (node = oceanus.documentCopy.getElementsByTagName('div')[nodeIndex]); nodeIndex++) {
            if (node.id !== 'undefined' && node.id != null && node.id != '') {
                for (var hindex = 0; hindex < hiddenElems.length; hindex++) {
                    var hiddenNode = hiddenElems[hindex];
                    if (hiddenNode.id !== 'undefined' && hiddenNode.id != null && hiddenNode.id != '' && hiddenNode.id == node.id) {
                        if (isHidden(hiddenNode)) {
                            if (node.parentNode != 'undefined' && node.parentNode != null) {
                                node.parentNode.removeChild(node);
                                nodeIndex--;
                            }
                        }
                    }
                }
            }
            else if (node.className !== 'undefined' && node.className != null && node.className != '') {
                for (var hindex = 0; hindex < hiddenElems.length; hindex++) {
                    var hiddenNode = hiddenElems[hindex];
                    if (hiddenNode.className !== 'undefined' && hiddenNode.className != null && hiddenNode.className != '' && hiddenNode.className == node.className) {
                        if (hiddenNode.innerHTML == node.innerHTML) {
                            if (node.parentNode != 'undefined' && node.parentNode != null) {
                                node.parentNode.removeChild(node);
                                nodeIndex--;
                            }
                        }
                    }
                }
            }
        }

        //remove all hidden images
        for (var nodeIndex = 0; (node = oceanus.documentCopy.getElementsByTagName('img')[nodeIndex]); nodeIndex++) {
            if (node.id !== 'undefined' && node.id != null && node.id != '') {
                for (var hindex = 0; hindex < hiddenImgElems.length; hindex++) {
                    var hiddenNode = hiddenImgElems[hindex];
                    if (hiddenNode.id !== 'undefined' && hiddenNode.id != null && hiddenNode.id != '' && hiddenNode.id == node.id) {
                        if (isHidden(hiddenNode)) {
                            if (node.parentNode != 'undefined' && node.parentNode != null) {
                                node.parentNode.removeChild(node);
                                nodeIndex--;
                            }
                        }
                    }
                }
            }
            else if (node.className !== 'undefined' && node.className != null && node.className != '') {
                for (var hindex = 0; hindex < hiddenImgElems.length; hindex++) {
                    var hiddenNode = hiddenImgElems[hindex];
                    if (hiddenNode.className !== 'undefined' && hiddenNode.className != null && hiddenNode.className != '' && hiddenNode.className == node.className) {
                        if (hiddenNode.innerHTML == node.innerHTML) {
                            if (node.parentNode != 'undefined' && node.parentNode != null) {
                                node.parentNode.removeChild(node);
                                nodeIndex--;
                            }
                        }
                    }
                }
            }
        }

        //clear array & free up memory
        hiddenElems = null;
        hiddenImgElems = null;

        /* Turn all double br's into p's */
        /* Note, this is pretty costly as far as processing goes. Maybe optimize later. */
        oceanus.documentCopy.innerHTML = oceanus.documentCopy.innerHTML.replace(oceanus.regexps.replaceBrsRe, '</p><p>').replace(oceanus.regexps.replaceFontsRe, '');
    },

    /**
    * Prepare the article node for display. Clean out any inline styles,
    * iframes, forms, strip extraneous <p> tags, etc.
    *
    * @param Element
    * @return void
    **/
    prepArticle: function (articleContent) {
        oceanus.cleanStyles(articleContent);
        oceanus.killBreaks(articleContent);

        /* Clean out junk from the article content */
        oceanus.clean(articleContent, "form");
        oceanus.clean(articleContent, "object");
        oceanus.clean(articleContent, "canvas");
        oceanus.clean(articleContent, "textarea");

        //clean H1
        for (var nodeIndex = 0; (node = articleContent.getElementsByTagName('h1')[nodeIndex]); nodeIndex++) {
            if (node.className != "oceanus-title") {
                node.parentNode.removeChild(node);
                --nodeIndex;
            }
        }

        //clean H2
        for (var nodeIndex = 0; (node = articleContent.getElementsByTagName('h2')[nodeIndex]); nodeIndex++) {
            if (node.className != "oceanus-title") {
                node.parentNode.removeChild(node);
                --nodeIndex;
            }
        }

        oceanus.clean(articleContent, "iframe");

        oceanus.cleanHeaders(articleContent);

        /* Do these last as the previous stuff may have removed junk that will affect these */
        if (articleContent.className != "" && articleContent.className == oceanusPostCSSClassName) {
            oceanus.cleanConditionally(articleContent, "table", true);
            oceanus.cleanConditionally(articleContent, "ul", true);
            oceanus.cleanConditionally(articleContent, "div", true);
        }
        else {
            oceanus.cleanConditionally(articleContent, "table", false);
            oceanus.cleanConditionally(articleContent, "ul", false);
            oceanus.cleanConditionally(articleContent, "div", false);
        }

        /* Remove extra paragraphs */
        var articleParagraphs = articleContent.getElementsByTagName('p');
        for (i = articleParagraphs.length - 1; i >= 0; i--) {
            var imgCount = articleParagraphs[i].getElementsByTagName('img').length;
            var embedCount = articleParagraphs[i].getElementsByTagName('embed').length;
            var objectCount = articleParagraphs[i].getElementsByTagName('object').length;

            if (imgCount == 0 && embedCount == 0 && objectCount == 0 && oceanus.getInnerText(articleParagraphs[i], false) == '') {
                articleParagraphs[i].parentNode.removeChild(articleParagraphs[i]);
            }
        }

        try {
            articleContent.innerHTML = articleContent.innerHTML.replace(/<br[^>]*>\s*<p/gi, '<p');
        }
        catch (e) {
            debug("Cleaning innerHTML of breaks failed. This is an IE strict-block-elements bug. Ignoring.");
        }

        sl_jQuery('a', articleContent).each(function () {
            var a = document.createElement('a');
            a.href = this.href;
            this.href = a.href;
            sl_jQuery(this).attr('onclick', '');
        });

        sl_jQuery('img', articleContent).each(function () {
            var j = new Image;
            j.src = this.src;

            if (j.width > 50 && j.height > 50) {

                //determine floats for the image
                for (var nodeIndex = 0; (node = document.body.getElementsByTagName('img')[nodeIndex]); nodeIndex++) {
                    if (node.src == this.src) {
                        try {
                            var nodeFloat = sl_jQuery(node).css('float');
                            var parentDivNodeFloat = '';
                            var parentDivNode = sl_jQuery(node).closest('div');
                            var thisParentDiv = sl_jQuery(this).closest('div');

                            if (parentDivNode != 'undefined') {
                                parentDivNodeFloat = sl_jQuery(parentDivNode).css('float');
                            }

                            if (nodeFloat == 'right' || nodeFloat == 'left') {
                                sl_jQuery(this).css('float', nodeFloat);
                            }
                            else if (parentDivNodeFloat == '' || parentDivNodeFloat == 'none') {
                                sl_jQuery(this).css('display', 'block');
                                sl_jQuery(this).css('margin-left', 'auto');
                                sl_jQuery(this).css('margin-right', 'auto');
                            }

                            if (parentDivNodeFloat == 'right' || parentDivNodeFloat == 'left') {
                                if (sl_jQuery(thisParentDiv).attr('class') != 'oceanus-content' && sl_jQuery(thisParentDiv).html() == parentDivNode.innerHTML) {
                                    sl_jQuery(thisParentDiv).css('float', parentDivNodeFloat);
                                }
                                else {
                                    sl_jQuery(this).css('float', parentDivNodeFloat);
                                }
                            }

                            if (sl_jQuery(node).width() != 'undefined') {
                                sl_jQuery(this).width(sl_jQuery(node).width());
                            }
                            if (sl_jQuery(node).height() != 'undefined') {
                                sl_jQuery(this).height(sl_jQuery(node).height());
                            }

                            if (sl_jQuery(node).css('padding') != 'undefined') {
                                sl_jQuery(this).css('padding', sl_jQuery(node).css('padding'));
                            }
                            if (sl_jQuery(node).css('padding-left') != 'undefined') {
                                sl_jQuery(this).css('padding-left', sl_jQuery(node).css('padding-left'));
                            }
                            if (sl_jQuery(node).css('padding-right') != 'undefined') {
                                sl_jQuery(this).css('padding-right', sl_jQuery(node).css('padding-right'));
                            }
                            if (sl_jQuery(node).css('padding-top') != 'undefined') {
                                sl_jQuery(this).css('padding-top', sl_jQuery(node).css('padding-top'));
                            }
                            if (sl_jQuery(node).css('padding-bottom') != 'undefined') {
                                sl_jQuery(this).css('padding-bottom', sl_jQuery(node).css('padding-bottom'));
                            }

                            if (sl_jQuery(parentDivNode).css('padding') != 'undefined') {
                                if (sl_jQuery(thisParentDiv).attr('class') != 'oceanus-content' && sl_jQuery(thisParentDiv).html() == parentDivNode.innerHTML) {
                                    sl_jQuery(thisParentDiv).css('padding', sl_jQuery(parentDivNode).css('padding'));
                                }
                                else {
                                    sl_jQuery(this).css('padding', sl_jQuery(parentDivNode).css('padding'));
                                }
                            }
                            if (sl_jQuery(parentDivNode).css('padding-left') != 'undefined') {
                                if (sl_jQuery(thisParentDiv).attr('class') != 'oceanus-content' && sl_jQuery(thisParentDiv).html() == parentDivNode.innerHTML) {
                                    sl_jQuery(thisParentDiv).css('padding-left', sl_jQuery(parentDivNode).css('padding-left'));
                                }
                                else {
                                    sl_jQuery(this).css('padding-left', sl_jQuery(parentDivNode).css('padding-left'));
                                }
                            }
                            if (sl_jQuery(parentDivNode).css('padding-right') != 'undefined') {
                                if (sl_jQuery(thisParentDiv).attr('class') != 'oceanus-content' && sl_jQuery(thisParentDiv).html() == parentDivNode.innerHTML) {
                                    sl_jQuery(thisParentDiv).css('padding-right', sl_jQuery(parentDivNode).css('padding-right'));
                                }
                                else {
                                    sl_jQuery(this).css('padding-right', sl_jQuery(parentDivNode).css('padding-right'));
                                }
                            }
                            if (sl_jQuery(parentDivNode).css('padding-top') != 'undefined') {
                                if (sl_jQuery(thisParentDiv).attr('class') != 'oceanus-content' && sl_jQuery(thisParentDiv).html() == parentDivNode.innerHTML) {
                                    sl_jQuery(thisParentDiv).css('padding-top', sl_jQuery(parentDivNode).css('padding-top'));
                                }
                                else {
                                    sl_jQuery(this).css('padding-top', sl_jQuery(parentDivNode).css('padding-top'));
                                }
                            }
                            if (sl_jQuery(parentDivNode).css('padding-bottom') != 'undefined') {
                                if (sl_jQuery(thisParentDiv).attr('class') != 'oceanus-content' && sl_jQuery(thisParentDiv).html() == parentDivNode.innerHTML) {
                                    sl_jQuery(thisParentDiv).css('padding-bottom', sl_jQuery(parentDivNode).css('padding-bottom'));
                                }
                                else {
                                    sl_jQuery(this).css('padding-bottom', sl_jQuery(parentDivNode).css('padding-bottom'));
                                }
                            }
                        }
                        catch (e) {
                        }
                    }
                }

                this.src = this.src; //trigger links to absolute URLs
            }
            else {
                this.parentNode.removeChild(this);
            }
        });

        var firstParagraphNode = sl_jQuery('p:first', articleContent)[0];
        if (firstParagraphNode != 'undefined' && firstParagraphNode != null) {
            var siblingNodes = firstParagraphNode.parentNode.childNodes;
            for (var k = 0; k < siblingNodes.length; ++k) {
                var childNode = siblingNodes[k];
                if (childNode == firstParagraphNode) {
                    sl_jQuery(firstParagraphNode).css('margin-top', '0');
                    break;
                }
                else if (childNode.nodeType == 3) { //TEXT NODE
                    break;
                }
            }
        }
    },

    /**
    * Initialize a node with the oceanus object. Also checks the
    * className/id for special names to add to its score.
    *
    * @param Element
    * @return void
    **/
    initializeNode: function (node) {
        node.oceanus = { "contentScore": 0 };

        switch (node.tagName) {
            case 'DIV':
                node.oceanus.contentScore += 5;
                break;

            case 'PRE':
            case 'TD':
            case 'BLOCKQUOTE':
                node.oceanus.contentScore += 3;
                break;

            case 'ADDRESS':
            case 'OL':
            case 'UL':
            case 'DL':
            case 'DD':
            case 'DT':
            case 'LI':
            case 'FORM':
                node.oceanus.contentScore -= 3;
                break;

            case 'H1':
            case 'H2':
            case 'H3':
            case 'H4':
            case 'H5':
            case 'H6':
            case 'TH':
                node.oceanus.contentScore -= 5;
                break;
        }

        node.oceanus.contentScore += oceanus.getClassWeight(node);
    },

    /***
    * grabArticle - Using a variety of metrics (content score, classname, element types), find the content that is
    *               most likely to be the stuff a user wants to read. Then return it wrapped up in a div.
    *
    * @return Element
    **/
    grabArticles: function () {
        var foundArticles = [];
        var foundPosts = [];
        var totalNumParagraphs = sl_jQuery('p', oceanus.documentCopy).length;

        /**
        * First, node prepping. Trash nodes that look cruddy (like ones with the class name "comment", etc), and turn divs
        * into P tags where they have been used inappropriately (as in, where they contain no other block level elements.)
        *
        * Note: Assignment from index for performance. See http://www.peachpit.com/articles/article.aspx?p=31567&seqNum=5
        * TODO: Shouldn't this be a reverse traversal?
        **/
        for (var nodeIndex = 0; (node = oceanus.documentCopy.getElementsByTagName('*')[nodeIndex]); nodeIndex++) {
            /* Remove unlikely candidates */
            var unlikelyMatchString = node.className + node.id;
            if (node.tagName !== "H1" && node.tagName !== "H2" && node.tagName !== "H3" && node.tagName !== "H4" && node.tagName !== "H5" && node.tagName !== "H6" &&
                unlikelyMatchString.search(oceanus.regexps.unlikelyCandidatesRe) !== -1 &&
				unlikelyMatchString.search(oceanus.regexps.maybeCandidateRe) == -1 &&
				node.tagName !== "BODY" ||
                (node.tagName == "A" && node.getAttribute('href') != null && node.getAttribute('href') !== 'undefined' && node.getAttribute('href').search(oceanus.regexps.unlikelyAdLinksRe) !== -1)) {

                if (node.tagName == "SPAN" && oceanus.getInnerText(node, true).length > 10 && document.title.indexOf(oceanus.getInnerText(node, true)) >= 0) {
                    continue;
                }

                if (sl_jQuery('p', node).length < (totalNumParagraphs * 0.5)) {
                    node.parentNode.removeChild(node);
                    nodeIndex--;
                    continue;
                }
            }

            /* Turn all divs that don't have children block level elements into p's */
            if (node.tagName === "DIV") {
                if (node.className.search(oceanus.regexps.postRe) !== -1) {
                    //don't alter div to p
                    if (sl_jQuery(node).attr('data-no-post') != 'true') {
                        foundPosts.push(node);
                        //mark children so that post subchildren aren't considered an independent post
                        sl_jQuery('div', node).attr('data-no-post', 'true');
                    }
                }
                else {
                    if (node.innerHTML.search(oceanus.regexps.divToPElementsRe) === -1) {
                        //Altering div to p
                        var newNode = document.createElement('p');
                        try {
                            newNode.innerHTML = node.innerHTML;
                            node.parentNode.replaceChild(newNode, node);
                            nodeIndex--;
                        }
                        catch (e) {
                            debug("Could not alter div to p, probably an IE restriction, reverting back to div.");
                        }
                    }
                    else if (node.innerHTML.search(oceanus.regexps.divToPElementsWithLinkRe) === -1) {
                        var linkDensity = oceanus.getLinkDensity(node);

                        if (linkDensity < 0.1) {
                            //Altering div to p
                            var newNode = document.createElement('p');
                            try {
                                newNode.innerHTML = node.innerHTML;
                                node.parentNode.replaceChild(newNode, node);
                                nodeIndex--;
                            }
                            catch (e) {
                                debug("Could not alter div to p, probably an IE restriction, reverting back to div.");
                            }
                        }
                        //                                                /* EXPERIMENTAL */
                        //                                                for (var i = 0, il = node.childNodes.length; i < il; i++) {
                        //                                                    var childNode = node.childNodes[i];
                        //                                                    if (childNode.nodeType == Node.TEXT_NODE) {
                        //                                                        debug("replacing text node with a p tag with the same content: " + childNode.nodeValue);
                        //                                                        var p = document.createElement('p');
                        //                                                        p.innerHTML = childNode.nodeValue;
                        //                                                        p.style.display = 'inline';
                        //                                                        p.className = 'oceanus-paragraph';
                        //                                                        childNode.parentNode.replaceChild(p, childNode);
                        //                                                    }
                        //                                                } 
                    }
                }
            }
        }

        if (foundPosts.length > 0) {
            //there are posts in this document

            //remove post from the document copy
            for (var postIndex = 0; postIndex < foundPosts.length; ++postIndex) {
                if (foundPosts[postIndex].parentNode != null && foundPosts[postIndex].parentNode !== 'undefined') {
                    foundPosts[postIndex].parentNode.removeChild(foundPosts[postIndex]);

                    var postTopCandidate = oceanus.scoreAndGetTopCandidate(foundPosts[postIndex]);
                    if (postTopCandidate != null && postTopCandidate !== 'undefined') {
                        //retrieve title/author/date of the post
                        var titleElem = null;
                        var postTitle = null;

                        //retrieve post title
                        for (var headerIndex = 1; headerIndex < 3; ++headerIndex) {
                            titleElem = foundPosts[postIndex].getElementsByTagName('h' + headerIndex)[0];
                            if (titleElem != null && titleElem !== 'undefined') {
                                break;
                            }
                        }

                        //process the post title and create a new h2 element
                        if (titleElem != null && titleElem !== 'undefined') {
                            postTitle = document.createElement('h2');
                            postTitle.className = "oceanus-title";
                            postTitle.innerHTML = oceanus.getInnerText(titleElem, true);

                            //check if title is embedded in a link
                            //check children
                            var checkTitleParents = true;
                            var parseTitleLink = getHeaderLink(titleElem, oceanus.getInnerText(titleElem, true));
                            if (parseTitleLink != null && parseTitleLink != 'undefined') {
                                var newPostTitle = document.createElement('a');
                                newPostTitle.href = parseTitleLink;
                                newPostTitle.appendChild(postTitle);
                                postTitle = newPostTitle;
                                checkTitleParents = false;
                            }

                            //check parent
                            if (checkTitleParents) {
                                var parseTitleElem = titleElem;
                                while (parseTitleElem.parentNode != null && parseTitleElem.parentNode != 'undefined') {
                                    if (parseTitleElem.parentNode.nodeName == "A") {
                                        var newPostTitle = document.createElement('a');
                                        newPostTitle.href = parseTitleElem.parentNode.href;
                                        newPostTitle.appendChild(postTitle);
                                        postTitle = newPostTitle;
                                        break;
                                    }
                                    parseTitleElem = parseTitleElem.parentNode;
                                }
                            }
                        }
                        else {
                            if (foundPosts.length == 1) {
                                postTitle = oceanus.getArticleTitle();
                            }
                        }

                        var topCandidateImages = [];
                        topCandidateImages = postTopCandidate.getElementsByTagName('img');

                        //                        //check quality of post images and encapsulate in paragraph if required
                        //                        for (var topCandidateImgIndex = 0; topCandidateImgIndex < topCandidateImages.length; ++topCandidateImgIndex) {
                        //                            if (topCandidateImages[topCandidateImgIndex].parentNode !== 'undefined' && topCandidateImages[topCandidateImgIndex].parentNode.tagName === "DIV" && 
                        //                                topCandidateImages[topCandidateImgIndex].parentNode.parentNode !== 'undefined') {
                        //                                
                        //                                if (topCandidateImages[topCandidateImgIndex].parentNode.childNodes.length == 1) {
                        //                                    //replacing image parent node with a p tag with the same content
                        //                                    var p = document.createElement('p');
                        //                                    p.style.display = 'inline';
                        //                                    topCandidateImages[topCandidateImgIndex].parentNode.parentNode.replaceChild(p, topCandidateImages[topCandidateImgIndex].parentNode);
                        //                                    p.appendChild(topCandidateImages[topCandidateImgIndex]);
                        //                                }
                        //                            }
                        //                        }

                        //retrieve all relevant post images
                        var postImages = [];
                        for (var imageIndex = 0, postImgElements = foundPosts[postIndex].getElementsByTagName('img'); imageIndex < postImgElements.length; ++imageIndex) {

                            if (postImgElements[imageIndex] != null && postImgElements[imageIndex] != "" && postImgElements[imageIndex].src.length > 5) {
                                var imgObj = new Image;
                                imgObj.src = postImgElements[imageIndex].src;

                                if (imgObj.width > 80 && imgObj.height > 80 && (imgObj.height > 109 || imgObj.width > 109)) {

                                    postImgElements[imageIndex].setAttribute("width", "");
                                    postImgElements[imageIndex].setAttribute("height", "");

                                    postImgElements[imageIndex].setAttribute("data-src", "");
                                    var topPostImageMatchFound = false;

                                    //check if this image is already part of the top candidate element
                                    for (var topCandidateImgIndex = 0; topCandidateImgIndex < topCandidateImages.length; ++topCandidateImgIndex) {
                                        if (topCandidateImages[topCandidateImgIndex].src == postImgElements[imageIndex].src) {
                                            topPostImageMatchFound = true;
                                            break;
                                        }
                                    }

                                    if (!topPostImageMatchFound) {
                                        postImages.push(postImgElements[imageIndex]);
                                    }
                                }
                            }
                        }

                        var postArticleContent = document.createElement("DIV");
                        postArticleContent.className = oceanusPostCSSClassName;

                        //append the title
                        if (postTitle != null && postTitle !== 'undefined') {
                            postArticleContent.appendChild(postTitle);
                        }

                        //append all images
                        for (var postImgIndex = 0; postImgIndex < postImages.length; ++postImgIndex) {
                            postArticleContent.appendChild(postImages[postImgIndex]);
                        }

                        //append main content
                        postArticleContent.appendChild(postTopCandidate);

                        //adjust image formatting
                        if (postArticleContent.getElementsByTagName('img').length == 1) {
                            postArticleContent.getElementsByTagName('img')[0].setAttribute("class", "oceanus-post-img");
                        }

                        foundArticles.push(postArticleContent);
                    }
                }
            }
        }

        //score and get top candidate for the document
        var topCandidate = oceanus.scoreAndGetTopCandidate(oceanus.documentCopy);

        /**
        * Now that we have the top candidate, look through its siblings for content that might also be related.
        * Things like preambles, content split by ads that we removed, etc.
        **/
        if (topCandidate != null && topCandidate !== 'undefined') {
            var articleContent = document.createElement("DIV");
            articleContent.className = "oceanus-content";

            articleContent.appendChild(oceanus.getArticleTitle());

            var siblingScoreThreshold = Math.max(10, topCandidate.oceanus.contentScore * 0.2);
            var siblingNodes = topCandidate.parentNode.childNodes;
            for (var i = 0, il = siblingNodes.length; i < il; i++) {
                var siblingNode = siblingNodes[i];
                var append = false;

                debug("Looking at sibling node: " + siblingNode + " (" + siblingNode.className + ":" + siblingNode.id + ")" + ((typeof siblingNode.oceanus != 'undefined') ? (" with score " + siblingNode.oceanus.contentScore) : ''));
                debug("Sibling has score " + (siblingNode.oceanus ? siblingNode.oceanus.contentScore : 'Unknown'));

                if (siblingNode === topCandidate) {
                    append = true;
                }

                if (typeof siblingNode.oceanus != 'undefined' && siblingNode.oceanus.contentScore >= siblingScoreThreshold) {
                    append = true;
                }

                if (siblingNode.nodeName == "P") {
                    var linkDensity = oceanus.getLinkDensity(siblingNode);
                    var nodeContent = oceanus.getInnerText(siblingNode);
                    var nodeLength = nodeContent.length;

                    if (nodeLength > 80 && linkDensity < 0.25) {
                        append = true;
                    }
                    else if (nodeLength < 80 && linkDensity == 0 && nodeContent.search(/\.( |$)/) !== -1) {
                        append = true;
                    }
                }

                if (append) {
                    debug("Appending node: " + siblingNode);

                    /* Append sibling and subtract from our list because it removes the node when you append to another node */
                    articleContent.appendChild(siblingNode);
                    i--;
                    il--;
                }
            }

            foundArticles.push(articleContent);
        }

        /**
        * So we have all of the content results that we need. Now we clean it up for presentation.
        **/
        for (var i = 0; i < foundArticles.length; ++i) {
            oceanus.prepArticle(foundArticles[i]);
        }

        return foundArticles;
    },

    scoreAndGetTopCandidate: function (e) {
        /**
        * Loop through all paragraphs, and assign a score to them based on how content-y they look.
        * Then add their score to their parent node.
        *
        * A score is determined by things like number of commas, class names, etc. Maybe eventually link density.
        **/
        var allParagraphs = e.getElementsByTagName("p");
        var candidates = [];
        var floatNodesChecked = [];

        for (var j = 0; j < allParagraphs.length; j++) {
            var parentNode = allParagraphs[j].parentNode;
            var grandParentNode = null;

            if (parentNode.className.search(oceanus.regexps.postRe) === -1) {
                grandParentNode = parentNode.parentNode;
            }

            var innerText = oceanus.getInnerText(allParagraphs[j]);

            /* If this paragraph is less than 50 characters, don't even count it. */
            if (innerText.length < 50)
                continue;

            /* Initialize oceanus data for the parent. */
            if (typeof parentNode.oceanus == 'undefined') {
                oceanus.initializeNode(parentNode);
                candidates.push(parentNode);
            }

            /* Initialize oceanus data for the grandparent. */
            if (grandParentNode != null && typeof grandParentNode.oceanus == 'undefined') {
                oceanus.initializeNode(grandParentNode);
                candidates.push(grandParentNode);
            }

            var contentScore = 0;

            /* Add a point for the paragraph itself as a base. */
            contentScore++;

            /* Add points for any commas within this paragraph */
            contentScore += innerText.split(',').length;

            /* For every 100 characters in this paragraph, add another point. Up to 3 points. */
            contentScore += Math.min(Math.floor(innerText.length / 100), 3);

            /* Add the score to the parent. The grandparent gets half. */
            parentNode.oceanus.contentScore += contentScore;

            var parentNodeEquiv = null;
            var grandParentNodeEquiv = null;

            if (floatNodesChecked.indexOf(parentNode) == -1) {
                floatNodesChecked.push(parentNode);

                if (parentNode.getElementsByTagName("p").length > 1) {
                    for (var z = 0; z < document.floatingParagraphs.length; z++) {
                        var parseParentNode = document.floatingParagraphs[z].parentNode;
                        var parseGrandParentNode = null;

                        if (parseParentNode != null) {
                            parseGrandParentNode = parseParentNode.parentNode;
                        }

                        if (parentNode != null && parseParentNode != null && parentNode.innerHTML == parseParentNode.innerHTML) {
                            parentNodeEquiv = parseParentNode;
                        }
                        if (grandParentNode != null && parseGrandParentNode != null && grandParentNode.innerHTML == parseGrandParentNode.innerHTML) {
                            grandParentNodeEquiv = parseGrandParentNode;
                        }
                    }
                }
            }

            if (parentNodeEquiv != null && sl_jQuery(parentNodeEquiv).css('float') != 'undefined' && (sl_jQuery(parentNodeEquiv).css('float') == 'right' || sl_jQuery(parentNodeEquiv).css('float') == 'left')) {
                if (grandParentNode != null) {
                    grandParentNode.removeChild(parentNode);
                }
            }

            if (grandParentNode != null) {
                grandParentNode.oceanus.contentScore += contentScore / 2;

                if (grandParentNodeEquiv != null && sl_jQuery(grandParentNodeEquiv).css('float') != 'undefined' && (sl_jQuery(grandParentNodeEquiv).css('float') == 'right' || sl_jQuery(grandParentNodeEquiv).css('float') == 'left')) {
                    candidates.splice(candidates.indexOf(grandParentNode, 1));
                }
            }
        }

        /**
        * After we've calculated scores, loop through all of the possible candidate nodes we found
        * and find the one with the highest score.
        **/
        var topCandidate = null;
        for (var i = 0, il = candidates.length; i < il; i++) {
            /**
            * Scale the final candidates score based on link density. Good content should have a
            * relatively small link density (5% or less) and be mostly unaffected by this operation.
            **/
            candidates[i].oceanus.contentScore = candidates[i].oceanus.contentScore * (1 - oceanus.getLinkDensity(candidates[i]));

            debug('Candidate: ' + candidates[i] + " (" + candidates[i].className + ":" + candidates[i].id + ") with score " + candidates[i].oceanus.contentScore);

            if (!topCandidate || candidates[i].oceanus.contentScore > topCandidate.oceanus.contentScore) {
                topCandidate = candidates[i];
            }
        }

        return topCandidate;
    },

    /**
    * Get the inner text of a node - cross browser compatibly.
    * This also strips out any excess whitespace to be found.
    *
    * @param Element
    * @return string
    **/
    getInnerText: function (e, normalizeSpaces) {
        var textContent = "";

        normalizeSpaces = (typeof normalizeSpaces == 'undefined') ? true : normalizeSpaces;

        if (navigator.appName == "Microsoft Internet Explorer")
            textContent = e.innerText.replace(oceanus.regexps.trimRe, "");
        else
            textContent = e.textContent.replace(oceanus.regexps.trimRe, "");

        if (normalizeSpaces)
            return textContent.replace(oceanus.regexps.normalizeRe, " ");
        else
            return textContent;
    },

    /**
    * Get the number of times a string s appears in the node e.
    *
    * @param Element
    * @param string - what to split on. Default is ","
    * @return number (integer)
    **/
    getCharCount: function (e, s) {
        s = s || ",";
        return oceanus.getInnerText(e).split(s).length;
    },

    /**
    * Remove the style attribute on every e and under.
    * TODO: Test if getElementsByTagName(*) is faster.
    *
    * @param Element
    * @return void
    **/
    cleanStyles: function (e) {

        if (!e)
            return;

        var cur = e.firstChild;

        // Remove any root styles, if we're able.
        if (typeof e.removeAttribute == 'function' && e.className != 'oceanus-paragraph' && e.className != 'oceanus-content' && e.className != 'oceanus-title' && e.className != 'oceanus-post-img') {
            e.removeAttribute('style');
            e.removeAttribute('id');
            e.removeAttribute('class');
        }

        e.removeAttribute('style');
        e.removeAttribute('alt');
        e.removeAttribute('title');

        // Go until there are no more child nodes
        while (cur != null) {
            if (cur.nodeType == 1) {
                // Remove style attribute(s) :                
                cur.removeAttribute("style");
                cur.removeAttribute("id");
                cur.removeAttribute('alt');
                cur.removeAttribute('title');

                if (cur.className != "oceanus-paragraph" && cur.className != "oceanus-content" && cur.className != "oceanus-title" && cur.className != "oceanus-post-img") {
                    cur.removeAttribute("class");
                }

                oceanus.cleanStyles(cur);
            }
            cur = cur.nextSibling;
        }
    },

    /**
    * Get the density of links as a percentage of the content
    * This is the amount of text that is inside a link divided by the total text in the node.
    * 
    * @param Element
    * @return number (float)
    **/
    getLinkDensity: function (e) {
        var links = e.getElementsByTagName("a");
        var textLength = oceanus.getInnerText(e).length;
        var linkLength = 0;
        for (var i = 0, il = links.length; i < il; i++) {
            linkLength += oceanus.getInnerText(links[i]).length;
        }

        return linkLength / textLength;
    },

    /**
    * Get an elements class/id weight. Uses regular expressions to tell if this 
    * element looks good or bad.
    *
    * @param Element
    * @return number (Integer)
    **/
    getClassWeight: function (e) {
        var weight = 0;

        /* Look for a special classname */
        if (e.className != "") {
            if (e.className.search(oceanus.regexps.negativeRe) !== -1)
                weight -= 25;

            if (e.className.search(oceanus.regexps.positiveRe) !== -1)
                weight += 25;
        }

        /* Look for a special ID */
        if (typeof (e.id) == 'string' && e.id != "") {
            if (e.id.search(oceanus.regexps.negativeRe) !== -1)
                weight -= 25;

            if (e.id.search(oceanus.regexps.positiveRe) !== -1)
                weight += 25;
        }

        return weight;
    },

    /**
    * Remove extraneous break tags from a node.
    *
    * @param Element
    * @return void
    **/
    killBreaks: function (e) {
        try {
            e.innerHTML = e.innerHTML.replace(oceanus.regexps.killBreaksRe, '<br />');
        }
        catch (e) {
            debug("KillBreaks failed - this is an IE bug. Ignoring.");
        }
    },

    /**
    * Clean a node of all elements of type "tag".
    * (Unless it's a youtube/vimeo video. People love movies.)
    *
    * @param Element
    * @param string tag to clean
    * @return void
    **/
    clean: function (e, tag) {
        var targetList = e.getElementsByTagName(tag);
        var isEmbed = (tag == 'object' || tag == 'embed');

        for (var y = targetList.length - 1; y >= 0; y--) {
            /* Allow youtube and vimeo videos through as people usually want to see those. */
            if (isEmbed && targetList[y].innerHTML.search(oceanus.regexps.videoRe) !== -1) {
                continue;
            }

            targetList[y].parentNode.removeChild(targetList[y]);
        }
    },

    /**
    * Clean an element of all tags of type "tag" if they look fishy.
    * "Fishy" is an algorithm based on content length, classnames, link density, number of images & embeds, etc.
    *
    * @return void
    **/
    cleanConditionally: function (e, tag, isPost) {
        var tagsList = e.getElementsByTagName(tag);
        var curTagsLength = tagsList.length;

        /**
        * Gather counts for other typical elements embedded within.
        * Traverse backwards so we can remove nodes at the same time without effecting the traversal.
        *
        * TODO: Consider taking into account original contentScore here.
        **/
        for (var i = curTagsLength - 1; i >= 0; i--) {
            var weight = oceanus.getClassWeight(tagsList[i]);

            if (weight < 0) {
                tagsList[i].parentNode.removeChild(tagsList[i]);
            }
            else if (oceanus.getCharCount(tagsList[i], ',') < 10) {
                /**
                * If there are not very many commas, and the number of
                * non-paragraph elements is more than paragraphs or other ominous signs, remove the element.
                **/

                var p = tagsList[i].getElementsByTagName("p").length;
                var img = tagsList[i].getElementsByTagName("img").length;
                var li = tagsList[i].getElementsByTagName("li").length - 100;
                var input = tagsList[i].getElementsByTagName("input").length;

                var embedCount = 0;
                var embeds = tagsList[i].getElementsByTagName("embed");
                for (var ei = 0, il = embeds.length; ei < il; ei++) {
                    if (embeds[ei].src.search(oceanus.regexps.videoRe) == -1) {
                        embedCount++;
                    }
                }

                var linkDensity = oceanus.getLinkDensity(tagsList[i]);
                var contentLength = oceanus.getInnerText(tagsList[i]).length;
                var toRemove = false;

                if (img > p && !isPost) {
                    toRemove = true;
                } else if (li > p && tag != "ul" && tag != "ol") {
                    toRemove = true;
                } else if (input > Math.floor(p / 3)) {
                    toRemove = true;
                } else if (contentLength < 25 && (img == 0 || img > 2)) {
                    toRemove = true;
                } else if (weight < 25 && linkDensity > .2 && !isPost) {
                    toRemove = true;
                } else if (weight < 25 && linkDensity > .5 && isPost) {
                    toRemove = true;
                } else if (weight >= 25 && linkDensity > .5) {
                    toRemove = true;
                } else if ((embedCount == 1 && contentLength < 75) || embedCount > 1) {
                    toRemove = true;
                }

                if (toRemove) {
                    tagsList[i].parentNode.removeChild(tagsList[i]);
                }
            }
        }
    },

    /**
    * Clean out spurious headers from an Element. Checks things like classnames and link density.
    *
    * @param Element
    * @return void
    **/
    cleanHeaders: function (e) {
        for (var headerIndex = 1; headerIndex < 7; headerIndex++) {
            var headers = e.getElementsByTagName('h' + headerIndex);
            for (var i = headers.length - 1; i >= 0; i--) {
                if (oceanus.getClassWeight(headers[i]) < 0) { // || oceanus.getLinkDensity(headers[i]) > 0.33) {
                    headers[i].parentNode.removeChild(headers[i]);
                }
            }
        }
    },

    htmlspecialchars: function (s) {
        if (typeof (s) == "string") {
            s = s.replace(/&/g, "&amp;");
            s = s.replace(/"/g, "&quot;");
            s = s.replace(/'/g, "&#039;");
            s = s.replace(/</g, "&lt;");
            s = s.replace(/>/g, "&gt;");
        }

        return s;
    }

};

function getHeaderLink(header, targetText) {
    if (header.childNodes != null && header.childNodes != 'undefined') {
        for (var index = 0; index < header.childNodes.length; ++index) {
            if (header.childNodes[index].nodeName == "A" && oceanus.getInnerText(header.childNodes[index], true) == targetText) {
                return header.childNodes[index].href;
            }
            else {
                var result = getHeaderLink(header.childNodes[index], targetText);
                if (result != null && result != 'undefined') {
                    return result;
                }
            }
        }
    }
}


///
/// MARKLET BEGINS FROM HERE
///

//var sl_domain = "http://localhost:2011";
//var sl_logindomain = "http://localhost:2011";
var sl_domain = "http://www.pipfly.com";
var sl_logindomain = "http://www.pipfly.com";
var sl_articleFormDomain = sl_domain + "/article/cliparticleform";
var sl_dashboardDomain = sl_domain + "/webpage/boomarkletdashboard";
var sl_videoClipDomain = sl_domain + "/clip/GetClipVideoUI";
var sl_photoClipDomain = sl_domain + "/clip/GetClipPhotoUI";
var sl_LoginDomain = sl_domain + "/button/login";
var sl_bookmarkletCSS = "#sl_RemoveLink { position: fixed; z-index: 2147483647; right: 0; top: 0; left: 0; height: 24px; padding: 12px 0; text-align: center; font-weight: bold; } #sl_overlay { position: fixed; z-index: 2147483647; top: 0; right: 0; bottom: 0; left: 0; background-color: #000; height: 100%; width: 100%; opacity: .9; } #sl_container { font-family: Helvetica, Arial, Verdana, sans-serif !important; height: 100%; position: absolute; padding-top: 5px; z-index: 2147483647; top: 0; left: 0; right:0; bottom:0; background-color: transparent; width: 100%; opacity: 1; } #sl_snipheaderouter { margin: 0px auto 0px auto; width: 752px; position: fixed; left:0; right:0; top:1px; z-index: 2147483647; -moz-border-radius: 5px; -webkit-border-radius: 5px; border-radius: 5px; overflow: auto; border: 1px solid #D9D9D9; background-color: transparent; } #sl_snipheader { margin: 0px auto 0px auto; width: 740px; padding: 5px; z-index: 2147483647; -moz-border-radius: 5px; -webkit-border-radius: 5px; border-radius: 5px; overflow: auto; border: 1px solid #fff; background-color: transparent; } #sl_snipheader #sl_snipheadercontent { background-color:#fff; border: 1px solid #D9D9D9; padding:5px; display:block; overflow:auto; } #sl_snipheader #sl_snipheadercontent #sl_snipheadercontenttext { font-weight:bold; font-size:12px; width:528px; float:left; height:24px; line-height:24px; font-family: Helvetica, Arial, Verdana, sans-serif !important; text-shadow: 2px 2px 5px #DDD; } #sl_snipheader #sl_snipheadercontent #sl_snipheadercontentbtn { width:200px; float:left; text-align:right; } #sl_container .sl_header { margin: 0px auto 0px auto; width: 940px; padding: 10px 10px 0 10px; -moz-border-radius-topleft: 10px; -webkit-border-top-left-radius: 10px; border-top-left-radius: 10px; -moz-border-radius-topright: 10px; -webkit-border-top-right-radius: 10px; border-top-right-radius: 10px; overflow: auto; border: solid #fff; border-width:1px 1px 0 1px; background-color: transparent; } #sl_container .sl_header .sl_session { width: 200px; height: 40px; padding:10px 0; position: relative; display: block; float: right; } #sl_userlogged, #sl_userlogged:hover { max-width: 200px; height: 20px; line-height: 20px; padding: 10px 0px; display: block; vertical-align: top; float: right; color: white; font-weight: bold; text-align: center; position: relative; } #sl_userlogged .sl_profileimg { margin-right: 6px; height: 20px; vertical-align: top; display: inline-block; } #sl_userlogged .sl_screenname { clear: both; line-height: 20px; height: 20px; font-size:12px; color:#555; font-weight: bold; display: inline-block; } #sl_iframecontent, #sl_sharelinkcontent, #sl_addphotoclipform, #sl_articleformiframe { height: 100%; margin:0px auto; width:960px; padding:0; overflow:auto; z-index: 2147483647; } #sl_photoselectform, #sl_addphotoclipform, #sl_sharelinkform, #sl_articleformiframe { background-color: transparent; margin: 0px auto; width: 940px; padding:0px 10px 10px 10px; overflow: hidden; border:solid #fff; border-width:0 1px 1px 1px; -moz-border-radius-bottomleft: 10px; -webkit-border-bottom-left-radius: 10px; border-bottom-left-radius: 10px; -moz-border-radius-bottomright: 10px; -webkit-border-bottom-right-radius: 10px; border-bottom-right-radius: 10px; } #sl_photoselect, #sl_videoselect, #sl_articleselect { margin: 0px auto; width: 940px; padding:0px 10px 10px 10px; background-color:transparent; overflow: hidden; border:solid #fff; border-width:0 1px 1px 1px; -moz-border-radius-bottomleft: 10px; -webkit-border-bottom-left-radius: 10px; border-bottom-left-radius: 10px; -moz-border-radius-bottomright: 10px; -webkit-border-bottom-right-radius: 10px; border-bottom-right-radius: 10px; z-index:2147483647; } #bookmarkpagediscusscontainer .page { min-height: 100%; height: 100%; position: relative; display: block; margin: 0 auto; width:940px; padding:0 10px 10px 10px; border: solid #fff; border-width:0 1px 1px 1px; -moz-border-radius-bottomleft: 10px; -webkit-border-bottom-left-radius: 10px; border-bottom-left-radius: 10px; -moz-border-radius-bottomright: 10px; -webkit-border-bottom-right-radius: 10px; border-bottom-right-radius: 10px; }  #bookmarkpagediscusscontainer .maincontentsect { background-color: #fff; margin: 0px auto; padding:10px 0; width: 940px; overflow: hidden; }  #sl_photoselecttitle { color: #444; font-size: 22px; line-height:22px; font-weight:normal; letter-spacing:1px; padding:10px; font-family: 'Rancho', cursive; border-top:1px solid #fff; border-bottom: 1px solid #D9D9D9; width:920px; overflow:auto; margin:0 auto; display:block; background-color:#f9f9f9; text-align:center; clear:both; text-shadow: 0 -1px rgba(34, 25, 25, 0.3); }  #sl_photoselect_middle { width:940px; overflow:auto; margin:0 auto; display:block; background-color:#fff; clear:both; }  #sl_articleselecttitle { color: #444; font-size: 22px; line-height:22px; font-weight:normal; letter-spacing:1px; padding:10px; font-family: 'Rancho', cursive; border-top:1px solid #fff; border-bottom: 1px solid #D9D9D9; width:920px; overflow:auto; margin:0 auto; display:block; background-color:#f9f9f9; text-align:center; clear:both; text-shadow: 0 -1px rgba(34, 25, 25, 0.3); }  #sl_articleselect_middle { width:940px; overflow:auto; margin:0 auto; display:block; background-color:#fff; clear:both; text-align:left; }  #sl_container .sl_menu { display:block; overflow:auto; padding:10px; height:60px; position:relative; background-color:#F9F9F9; border-bottom:1px solid #d9d9d9; }  #sl_container .sl_logo { height:60px; margin:0 10px; display:block; float:left; width:115px; }  #sl_container .sl_logo img { border:none; }  #sl_container .sl_menubtns { padding:0; margin:0; border:none; width:540px; position:absolute; overflow:auto; height:60px; left:190px; }  #sl_container .pageprofile { margin: 10px auto 0 auto; width: 920px; padding: 10px; overflow: auto; background-color: #f5f5f5; }  .sl_photo_select_cont { position: relative; padding: 0; margin: 0; float: left; background-color: white; border: solid #d9d9d9; background-color:#fff; height: 234px; width: 234px; z-index: 2147483647; text-align: center; border-width: 0 1px 1px 0; overflow:hidden; }  .sl_photo_select_cont a, .sl_photo_select_cont a:hover, .sl_photo_select_cont a:visited, .sl_photo_select_cont a:active { text-decoration:none; }  .sl_photo_select_cont .sl_photo_select_img { max-height: 234px; max-width: 234px; width: auto !important; height: auto !important; }  .sl_photo_select_cont .sl_photo_select_img_caption, .sl_photo_select_cont .sl_photo_select_img_caption:hover { position: absolute; bottom: 15px; left:0; right:0; width:70px; color:#444; margin: 0 auto; text-decoration:none; text-align: center; font-size: 10px; z-index:2147483647; background: white; border-radius: 4px; padding: 0 2px; }  .statpaddingleft { padding-left: 15px; }  .pageprofile .pageinfo { text-align: left; overflow: auto; }  .pageprofile .pageinfo .name { font-size: 16px; text-align: left; font-weight: bold; color: #606060; text-shadow: 2px 2px 5px #DDD; }  .pageprofile .pageinfo .url { font-size: 12px; text-align: left; padding-top: 5px; color: #888; font-weight: normal; }  .maincontentsect .discussionsect { width: 570px; padding: 20px; border-right: 1px solid #d9d9d9; margin-right: 20px; text-align: left; float: left; overflow: hidden; }  .maincontentsect .othercontentsect { float: left; width: 290px; padding:10px 10px 15px 0; }  #sl_container .sl_menubtns .sl_btn img, #sl_container .sl_menubtns .closebtn img { margin:0; padding:0; clear:both; }  #sl_container .sl_menubtns .sl_btn, #sl_container .sl_menubtns .sl_btn:link, #sl_container .sl_menubtns .sl_btn:active, #sl_container .sl_menubtns .sl_btn:visited, #sl_container .sl_menubtns .sl_btn:hover, #sl_container .sl_menubtns .closebtn, #sl_container .sl_menubtns .closebtn:link, #sl_container .sl_menubtns .closebtn:active, #sl_container .sl_menubtns .closebtn:visited, #sl_container .sl_menubtns .closebtn:hover { font-family: Helvetica, Arial, Verdana, sans-serif !important; letter-spacing:normal; font-size: 12px; width: 94px; -webkit-box-shadow: none; -moz-box-shadow: none; box-shadow: none; padding: 0 3px; margin:0; line-height:16px; border:none !important; float: left; color: #606060; text-decoration:none; text-align: center; font-weight:bold; text-shadow: 2px 2px 2px #ddd; cursor:pointer; background-color:transparent; }  #sl_container .sl_menubtns .closebtn, #sl_container .sl_menubtns .closebtn:link, #sl_container .sl_menubtns .closebtn:active, #sl_container .sl_menubtns .closebtn:visited { margin-left:20px; padding-left:20px; border-left:1px solid #d9d9d9; }  .maincontentsect .graytext { color:#aaa; background-color:transparent; }  #sl_container .sl_menubtns .sl_btn:hover { color:#106fb8; background-color:transparent; text-shadow: 2px 2px 5px #DDD; }  /** BUTTON / EMPTY TEXT STYLES **/ .btnredstyle, .btnredstyle:link, .btnredstyle:hover, .btnredstyle:visited, .btnstyle, .btnstyle:link, .btnstyle:hover, .btnstyle:visited, .btnundo, .btnundo:link, .btnundo:hover, .btnundo:visited, .btnbluestyle, .btnbluestyle:link, .btnbluestyle:hover, .btnbluestyle:visited, .btngreenstyle, .btngreenstyle:link, btngreenstyle:hover, btngreenstyle:visited, .btnyellowstyle, .btnyellowstyle:link, .btnyellowstyle:hover, .btnyellowstyle:visited, .disabledbtn, .disabledbtn:link, .disabledbtn:hover, .disabledbtn:hover:active, .disabledbtn:visited { -moz-border-radius: 3px; -webkit-border-radius: 3px; border-radius: 3px; font-variant: normal; padding: 4px 5px; cursor: pointer; font-style:normal; box-shadow: 0 1px 2px #d9d9d9; -webkit-box-shadow: 0 1px 2px #d9d9d9; position: relative; text-align: center; font-weight: bold; font-size: 12px; line-height: normal !important; white-space: nowrap; border: 1px solid #aaa; display: inline-block; text-decoration: none; vertical-align: middle; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnstyle .imgleft { padding-right: 5px; position: relative; top: 1px; }  .btnundo, .btnundo:visited, .disabledbtn { color: #444; font-weight: normal; border: 1px solid #d9d9d9; background-color: #e9e9e9; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .disabledbtn, .disabledbtn:hover, .disabledbtn:active, .disabledbtn:visited { font-style:italic; cursor:default; color:#444; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnundo:hover { color: #fff; font-weight: normal; background-color: #a9a9a9; border: 1px solid #aaa; text-decoration: none; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnstyle, .btnstyle:link, .btnstyle:visited, .uploadifyButton, .uploadifyButton:visited { color: #444; background: url('../images/sitetheme/btn_gray_background.png'); font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnstyle:hover, .uploadifyButton:hover { color: #106fb8; text-decoration: none; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btngreenstyle, .btngreenstyle:link, .btngreenstyle:visited, .btngreenstyle:hover { background: #698B22; background: -webkit-gradient(linear,left top,left bottom,from(#9ACD32),to(#698B22)); background: -moz-linear-gradient(top,#9ACD32,#698B22); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#9ACD32',endColorstr='#698B22'); border: 1px solid #698B22; box-shadow: 0 1px 2px #f0f0f0; text-decoration:none; -webkit-box-shadow: 0 1px 2px #f0f0f0; color: #fff; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnredstyle, .btnredstyle:link, .btnredstyle:visited, .btnredstyle:hover { background: #106fb8; background: -webkit-gradient(linear,left top,left bottom,from(#EE30A7),to(#CD2990)); background: -moz-linear-gradient(top,#EE30A7,#CD2990); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#EE30A7',endColorstr='#CD2990'); border: 1px solid #CD2990; box-shadow: 0 1px 2px #f0f0f0; -webkit-box-shadow: 0 1px 2px #f0f0f0; color: #fff; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnbluestyle, .btnbluestyle:link, .btnbluestyle:visited { background: #106fb8; background: -webkit-gradient(linear,left top,left bottom,from(#0296E8),to(#106fb8)); background: -moz-linear-gradient(top,#0296E8,#106fb8); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0296E8',endColorstr='#106fb8'); border: 1px solid #106fb8; box-shadow: 0 1px 2px #f0f0f0; -webkit-box-shadow: 0 1px 2px #f0f0f0; color: #fff; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnbluestyle:hover { color: #fff; text-decoration: none; background: #106fb8; background: -webkit-gradient(linear,left top,left bottom,from(#0296E8),to(#106fb8)); background: -moz-linear-gradient(top,#0296E8,#106fb8); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0296E8',endColorstr='#106fb8'); box-shadow: 0 1px 2px #f0f0f0; -webkit-box-shadow: 0 1px 2px #f0f0f0; border: 1px solid #888; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .btnyellowstyle, .btnyellowstyle:link, .btnyellowstyle:visited, .btnyellowstyle:hover { background: #FFB72E; background: -webkit-gradient(linear,left top,left bottom,from(#FFEE66),to(#FFB72E)); background: -moz-linear-gradient(top,#FFEE66,#FFB72E); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#FFEE66',endColorstr='#FFB72E'); border: 1px solid #FFAA22; box-shadow: 0 1px 2px #f0f0f0; -webkit-box-shadow: 0 1px 2px #f0f0f0; text-shadow: 0 1px 0 #FE6; color: #444; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .emptytextstyle { color: #888; font-family: Helvetica, Arial, Verdana, sans-serif !important; font-weight:normal; }  .oceanus-content { padding:10px; margin:0 10px; border-bottom:1px double #d9d9d9; border-bottom-width:3px; clear:both; overflow:auto; color:#444; font-size:14px; font-weight:normal; font-family: Helvetica, Arial, Verdana, sans-serif !important; text-shadow:none; font-style:normal; font-variant:normal; }  .oceanus-content a:link, .oceanus-content a:active, .oceanus-content a:visited, .oceanus-content a:hover, .oceanus-content a { font-size:14px; font-weight:normal; font-family: Helvetica, Arial, Verdana, sans-serif !important; text-shadow:none; font-style:normal; font-variant:normal; color: #106fb8; margin-left:10px; }  .oceanus-content p, .oceanus-content span { font-size:14px; line-height:20px; font-weight:normal; font-family: Helvetica, Arial, Verdana, sans-serif !important; text-shadow:none; font-style:normal; color:#444; font-variant:normal; margin:14px 0; }  .oceanus-content img { text-align:center; margin:0 auto; margin-right:10px; margin-bottom:10px; background:#fff; }  .oceanus-content .oceanus-post-img { clear:both; float:left; margin-right:10px; margin-bottom:10px; background:#fff; }  .oceanus-content .oceanus-title, .oceanus-content h2 { text-align:left; color:#444; font-size:18px; line-height:22px; font-weight:bold; padding-bottom:10px; margin:0; font-style:normal; font-variant:normal; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .oceanus-content h3, .oceanus-content h4, .oceanus-content h5, .oceanus-content h6 { font-size:14px; line-height:16px; font-weight:bold; font-family: Helvetica, Arial, Verdana, sans-serif !important; }  .oceanus-content-link, .oceanus-content-link:hover, .oceanus-content-link:link, .oceanus-content-link:active { float:right; background: #106fb8; background: -webkit-gradient(linear,left top,left bottom,from(#0296E8),to(#106fb8)); background: -moz-linear-gradient(top,#0296E8,#106fb8); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0296E8',endColorstr='#106fb8'); border: 1px solid #106fb8; box-shadow: 0 1px 2px #f0f0f0; -webkit-box-shadow: 0 1px 2px #f0f0f0; color:#fff; font-size:16px; padding:3px 6px; font-weight:bold; -moz-border-radius: 4px; -webkit-border-radius: 4px; border-radius: 4px; font-weight:bold; font-family: Helvetica, Arial, Verdana, sans-serif !important; }";
var sl_clipphotoformContainer;
var sl_sharelinkContainer;
var createScrapbookSectionEmptyText = "Type a new scrapbook section here...";
var describeClipEmptyText = "Say something about the photo...";
var shareLinkRecipientEmptyText = "Type the name of your friends here...";
var sl_jQuery = null;
var sl_reloadDiscussion = true;

function removejscssfile(filename, filetype) {
    var targetelement = (filetype == "js") ? "script" : (filetype == "css") ? "link" : "none";  //determine element type to create nodelist from
    var targetattr = (filetype == "js") ? "src" : (filetype == "css") ? "href" : "none";  //determine corresponding attribute to test for
    var allsuspects = document.getElementsByTagName(targetelement);
    for (var i = allsuspects.length; i >= 0; i--) { //search backwards within nodelist for matching elements to remove
        if (allsuspects[i] && allsuspects[i].getAttribute(targetattr) != null && allsuspects[i].getAttribute(targetattr).indexOf(filename) != -1)
            allsuspects[i].parentNode.removeChild(allsuspects[i]) //remove element by calling parentNode.removeChild()
    }

    jQuery = document.oldJquery;

    if (document.forms['sl_loadwebpageform'] != 'undefined' && document.forms['sl_loadwebpageform'] != null) {
        document.body.removeChild(document.forms['sl_loadwebpageform']);
    }
    if (document.forms['sl_articleform'] != 'undefined' && document.forms['sl_articleform'] != null) {
        document.body.removeChild(document.forms['sl_articleform']);
    }
    if (document.forms['sl_loadvideoclipform'] != 'undefined' && document.forms['sl_loadvideoclipform'] != null) {
        document.body.removeChild(document.forms['sl_loadvideoclipform']);
    }
    if (document.forms['sl_loadphotoclipform'] != 'undefined' && document.forms['sl_loadphotoclipform'] != null) {
        document.body.removeChild(document.forms['sl_loadphotoclipform']);
    }
}

// dynamically load any javascript file.
function sl_loadJQuery() {
    var s = document.createElement('script');
    s.setAttribute("type", "text/javascript");
    s.setAttribute("src", sl_domain + "/scripts/plugins/jquery-1.7.1.min.js");

    if (typeof s.readyState === 'undefined') {
        s.onload = function () {
            sl_LoadPostMessages();
        };
    } else { // IE LAST!
        s.onreadystatechange = function () {
            if (s.readyState === "loaded" || s.readyState === "complete") {
                s.onreadystatechange = null;
                sl_LoadPostMessages();
            }
        };
    };

    document.body.appendChild(s);
}

function sl_LoadPostMessages() {
    var s = document.createElement('script');
    s.setAttribute("type", "text/javascript");
    s.setAttribute("src", sl_domain + "/scripts/plugins/jquery.postmessages.js");

    if (typeof s.readyState === 'undefined') {
        s.onload = function () {
            sl_jQuery = jQuery.noConflict(true);
            sl_initPipfly();
        };
    } else { // IE LAST!
        s.onreadystatechange = function () {
            if (s.readyState === "loaded" || s.readyState === "complete") {
                s.onreadystatechange = null;
                sl_jQuery = jQuery.noConflict(true);
                sl_initPipfly();
            }
        };
    };

    document.body.appendChild(s);
}

function sl_initPipfly() {
    sl_jQuery.receiveMessage(
        function (e) {
            if (e.data.indexOf('if_height') >= 0) {
                var h = Number(e.data.replace(/.*if_height=(\d+)(?:&|$)/, '$1'));
                if (!isNaN(h) && h > 0) {
                    if (showingArticleClip) {
                        var articleIFrame = document.getElementById('sl_articleformiframe');
                        sl_jQuery(articleIFrame).css("height", h + 'px');
                    }
                }
            }
            else if (e.data.indexOf('sl_login') >= 0) {
                var h = Number(e.data.replace(/.*sl_login=(\d+)(?:&|$)/, '$1'));
                if (h == 1) {
                    sl_checkIfUserLoggedIn();
                }
            }
        },
        sl_domain
    );

    sl_loadSLBookmarklet();
}

(function () {
    if (window.location.href == 'undefined' || window.location.href == null || window.location.href == 'about:blank' || window.location.href == '') {
        alert("We're sorry but an error occured in trying to load the pipfly bookmarklet.");
        return;
    }

    if (!document.sl_bookmarkletLoaded) {
        document.sl_bookmarkletLoaded = 1;

        var s = document.createElement('link');
        s.setAttribute("href", "http://fonts.googleapis.com/css?family=Rancho");
        s.setAttribute("rel", "stylesheet");
        s.setAttribute("type", "text/css");
        document.body.appendChild(s);

        sl_loadJQuery();
    }
})();

function sl_closePipfly() {
    sl_jQuery('#sl_overlay').remove();
    sl_jQuery('#sl_container').remove();
    removejscssfile(sl_domain + "/scripts/pipfly.marklet.js", "js");
    removejscssfile(sl_domain + "/scripts/plugins/jquery-1.7.1.min.js", "js");
    removejscssfile(sl_domain + "/scripts/plugins/jquery.postmessages.js", "js");
    removejscssfile("http://fonts.googleapis.com/css?family=Rancho", "css");
    document.sl_bookmarkletLoaded = 0;
    document.sl_number = 0;

    var embedTags = document.getElementsByTagName("embed");
    for (k = 0; k < embedTags.length; k++) {
        sl_jQuery(embedTags[k]).show();
    }

    return false;
}

function sl_checkIfUserLoggedIn() {
    var checkUrl = sl_domain + "/account/IsUserLoggedIn";

    sl_jQuery.ajax({
        url: checkUrl,
        dataType: "jsonp",
        timeout: 5000,
        success: function (data, status) {
            if (data.successful == "true") {
                sl_loadBookmarkletContents(data.fullname, data.profileimageurl);
            }
            else {
                sl_showBookmarkletLogin();
            }
        },
        error: function (XHR, textStatus, errorThrown) {
        }
    });
}

function sl_showBookmarkletLogin() {
    var n = document.getElementById("sl_container");
    n.innerHTML = '<div class="sl_header" style="width:320px;"><div class="sl_menu" style="width:300px;"><div class="sl_logo"><img style="display:inline;border:none;padding:0;margin:0;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:108px;height:auto;float:none;" src="' + sl_domain + '/images/sitetheme/pipfly_logo_sm.png" alt /></div><div class="sl_menubtns" style="left:auto;right:10px;width:140px;"><a class="closebtn" href="#" onclick="javascript:sl_closePipfly();return false;"><img style="float:none;border:none;padding:0;margin:0;clear:both;display:inline;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:40px;height:40px;" src="' + sl_domain + '/images/bookmarklet/close.png" /><br />Close pipfly</a></div></div></div>';
    sl_showPipflyLogin();
    window.scroll(0, 0);
}

function sl_loadBookmarkletContents(userfn, userpimg) {
    var n = document.getElementById("sl_container");
    n.innerHTML = '<div class="sl_header"><div class="sl_menu"><div class="sl_logo"><img style="display:inline;border:none;padding:0;margin:0;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:108px;height:auto;float:none;" src="' + sl_domain + '/images/sitetheme/pipfly_logo_sm.png" alt /></div><div class="sl_menubtns"><a href="#" class="sl_btn" onclick="javascript: sl_showDiscussion();"><img style="float:none;border:none;padding:0;margin:0;clear:both;display:inline;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:40px;height:40px;" src="' + sl_domain + '/images/bookmarklet/dashboard.png" /><br />Dashboard</a><a href="#" class="sl_btn" onclick="javascript:sl_showArticleClipUI();return false;"><img style="float:none;border:none;padding:0;margin:0;clear:both;display:inline;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:40px;height:40px;" src="' + sl_domain + '/images/bookmarklet/story.png" /><br />Clip Article</a><a class="sl_btn" href="#" onclick="javascript:sl_showPhotoSelect();return false;"><img style="float:none;border:none;padding:0;margin:0;clear:both;display:inline;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:40px;height:40px;" src="' + sl_domain + '/images/bookmarklet/photo.png" /><br />Clip Photo</a><a class="sl_btn" href="#" onclick="javascript:sl_showVideoSelect();return false;"><img style="float:none;border:none;padding:0;margin:0;clear:both;display:inline;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:40px;height:40px;" src="' + sl_domain + '/images/bookmarklet/video.png" /><br />Clip Video</a><a class="closebtn" href="#" onclick="javascript:sl_closePipfly();return false;"><img style="float:none;border:none;padding:0;margin:0;clear:both;display:inline;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;width:40px;height:40px;" src="' + sl_domain + '/images/bookmarklet/close.png" /><br />Close pipfly</a></div><div class="sl_session"><a id="sl_userlogged"><img src="' + userpimg + '" alt="" style="display:inline;box-shadow:none;-webkit-box-shadow:none;-moz-box-shadow:none;height:20px;" class="sl_profileimg"><span class="sl_screenname">' + userfn + '</span></a></div></div></div>';

    sl_reloadDiscussion = true;
    sl_showDiscussion();
    window.scroll(0, 0);
}

function sl_loadSLBookmarklet() {
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (elt /*, from*/) {
            var len = this.length >>> 0;

            var from = Number(arguments[1]) || 0;
            from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
            if (from < 0)
                from += len;

            for (; from < len; from++) {
                if (from in this &&
          this[from] === elt)
                    return from;
            }
            return -1;
        };
    }

    adjustEmbedObjs();

    try {
        oceanus.init();
    }
    catch (e) {
        alert('error');
    }

    var f = document.createElement("style");
    f.setAttribute("type", "text/css");
    if (f.styleSheet) { //FOR IE
        f.styleSheet.cssText = sl_bookmarkletCSS;
    }
    else {
        f.innerHTML = sl_bookmarkletCSS;
    }
    document.getElementsByTagName('head')[0].appendChild(f);
    var overlay = document.createElement("div");
    overlay.setAttribute("id", "sl_overlay");
    document.body.appendChild(overlay);
    var n = document.createElement("div");
    n.setAttribute("id", "sl_container");
    document.body.appendChild(n);

    sl_jQuery('#sl_overlay').height(document.height);
    sl_checkIfUserLoggedIn();

    scroll(0, 0);

    var embedTags = document.getElementsByTagName("embed");
    for (k = 0; k < embedTags.length; k++) {
        sl_jQuery(embedTags[k]).hide();
    }
}

function adjustYouTubeContainer(g) {
    if (g.src && g.src != "") {
        var j = g.src.indexOf("?") > -1 ? "&" : "?";
        g.src += j + "autoplay=0";
        g.src += "&wmode=transparent"
    }
    g.setAttribute("wmode", "transparent");
    j = g.parentNode;
    var r = g.nextSibling;
    j.removeChild(g);
    j.insertBefore(g, r)
}

function adjustEmbedObjs() {
    var youTubeRegEx = null;
    var iframeTags = document.getElementsByTagName("iframe");
    for (k = 0; k < iframeTags.length; k++) {
        youTubeRegEx = /^http:\/\/www\.youtube\.com\/embed\/([a-zA-Z0-9\-_]+)/;
        if (youTubeRegEx = youTubeRegEx.exec(iframeTags[k].src)) {
            adjustYouTubeContainer(iframeTags[k]);
        }
    }

    var embedTags = document.getElementsByTagName("embed");
    for (k = 0; k < embedTags.length; k++) {
        youTubeRegEx = /^http:\/\/www\.youtube\.com\/v\/([a-zA-Z0-9\-_]+)/;
        if (youTubeRegEx = youTubeRegEx.exec(embedTags[k].src)) {
            adjustYouTubeContainer(embedTags[k]);
        }
    }

    youTubeRegEx = /^http:\/\/www\.youtube\.com\/watch\?v=([a-zA-Z0-9\-_]+)/;
    if (youTubeRegEx = youTubeRegEx.exec(window.location.href)) {
        adjustYouTubeContainer(document.getElementById("movie_player"));
    }
}

function sl_showVideoSelect() {
    showingArticleClip = false;

    var results = [];
    var videoIds = [];

    function youTubeImg(g) {
        var j = new Image;
        j.height = 360;
        j.width = 480;
        j.src = "http://img.youtube.com/vi/" + g + "/0.jpg";
        return j;
    }

    var youTubeRegEx = null;
    var iframeTags = document.getElementsByTagName("iframe");
    for (k = 0; k < iframeTags.length; k++) {
        youTubeRegEx = /^http:\/\/www\.youtube\.com\/embed\/([a-zA-Z0-9\-_]+)/;
        if (youTubeRegEx = youTubeRegEx.exec(iframeTags[k].src)) {
            results.push(youTubeImg(youTubeRegEx[1]));
            videoIds.push(youTubeRegEx[1]);
        }
    }

    var embedTags = document.getElementsByTagName("embed");
    for (k = 0; k < embedTags.length; k++) {
        youTubeRegEx = /^http:\/\/www\.youtube\.com\/v\/([a-zA-Z0-9\-_]+)/;
        if (youTubeRegEx = youTubeRegEx.exec(embedTags[k].src)) {
            results.push(youTubeImg(youTubeRegEx[1]));
            videoIds.push(youTubeRegEx[1]);
        }
    }

    var imgTags = document.images;
    for (k = 0; k < imgTags.length; k++) {
        youTubeRegEx = /^http:\/\/i\.ytimg\.com\/vi\/([a-zA-Z0-9\-_]+)/;
        if (youTubeRegEx = youTubeRegEx.exec(imgTags[k].src)) {
            results.push(youTubeImg(youTubeRegEx[1]));
            videoIds.push(youTubeRegEx[1]);
        }
    }

    youTubeRegEx = /^http:\/\/www\.youtube\.com\/watch\?v=([a-zA-Z0-9\-_]+)/;
    if (youTubeRegEx = youTubeRegEx.exec(window.location.href)) {
        results.push(youTubeImg(youTubeRegEx[1]));
        videoIds.push(youTubeRegEx[1]);
    }

    if (results.length == 0) {
        window.alert("Sorry, we can't find any supported videos on this page.");
    }
    else {
        sl_showVideoClipUI(results, videoIds);
    }

    return false;
}

function sl_showPhotoSelect() {
    showingArticleClip = false;

    var results = [];
    var i = {};

    for (k = 0; k < document.images.length; k++) {
        var img = document.images[k];
        if (img.style.display != "none") {
            img = { width: img.width, height: img.height, src: img.src };
            if (img.width > 80 && img.height > 80 && (img.height > 109 || img.width > 109)) {
                var imgObj = new Image;
                imgObj.src = img.src;

                if (!(imgObj.height && imgObj.height < 80) && !i[img.src]) {
                    i[img.src] = 1;
                    results.push(img)
                }
            }
        }
    }

    sl_jQuery('li:visible').each(function (index) {
        if (sl_jQuery(this).css('background-image') != null && sl_jQuery(this).css('background-image') != "none") {
            var imageUrl = sl_jQuery(this).css('background-image').replace(/"/g, "").replace(/url\(|\)$/ig, "");

            if (imageUrl != null && imageUrl != "" && imageUrl.substring(0, 5) == "http:") {
                var imgObj = new Image;
                imgObj.src = imageUrl;

                var img = { width: imgObj.width, height: imgObj.height, src: imageUrl };
                if (img.width > 80 && img.height > 80 && (img.height > 109 || img.width > 109) && !i[img.src]) {
                    i[img.src] = 1;
                    results.push(img)
                }
            }
        }
    });

    if (results.length == 0) {
        window.alert("Sorry, we can't find any large images on this page.");
    }
    else {
        sl_showPhotoClipUI(results);
    }

    return false;
}

function sl_getPageDescription() {
    var description;
    var metas = document.getElementsByTagName('meta');
    for (var x = 0, y = metas.length; x < y; x++) {
        if (metas[x].name.toLowerCase() == "description" || metas[x].name.toLowerCase() == "og:description") {
            description = metas[x].getAttribute('content');
            break;
        }
    }

    return description;
}

function sl_showArticleClipForm(articleIndex) {
    var sl_clipArticleContainer;
    var sl_clipArticleForm;

    if ((sl_clipArticleContainer = document.getElementById('sl_articleformiframe')) != null) {
        sl_clipArticleContainer.parentNode.removeChild(sl_clipArticleContainer);
        sl_clipArticleContainer = null;
    }

    if ((sl_clipArticleForm = document.getElementById('sl_articleform')) != null) {
        sl_clipArticleForm.parentNode.removeChild(sl_clipArticleForm);
        sl_clipArticleForm = null;
    }

    sl_clipArticleContainer = document.createElement("iframe");
    sl_clipArticleContainer.setAttribute("id", "sl_articleformiframe");
    sl_clipArticleContainer.setAttribute("name", "sl_articleformiframe");
    sl_clipArticleContainer.setAttribute("allowtransparency", "true");
    sl_clipArticleContainer.setAttribute("frameborder", "0");
    sl_clipArticleContainer.setAttribute("style", "width:962px;clear:both;border:none;display:block;padding: 0 !important;margin:0 auto !important;");
    sl_clipArticleContainer.setAttribute("scrolling", "no");
    document.getElementById('sl_container').appendChild(sl_clipArticleContainer);

    var articleContentHtml = document.articles[articleIndex].cloneNode(true);
    var articleImages = articleContentHtml.getElementsByTagName('img');
    for (i = articleImages.length - 1; i >= 0; i--) {
        var imgObj = new Image;
        imgObj.src = articleImages[i].src;

        articleImages[i].removeAttribute('width');
        articleImages[i].removeAttribute('height');
        if (imgObj.width > 0) {
            sl_jQuery(articleImages[i]).css('width', imgObj.width);
        }
        if (imgObj.height > 0) {
            sl_jQuery(articleImages[i]).css('height', imgObj.height);
        }
    }

    var articleDiv = document.createElement("div");
    articleDiv.appendChild(articleContentHtml);

    var articleTitle = "";
    var articleTitleNode = articleContentHtml.getElementsByTagName('h2');

    if (articleTitleNode != null && articleTitleNode != 'undefined') {
        articleTitle = oceanus.getInnerText(articleTitleNode[0], true);
    }

    var articleText = oceanus.getInnerText(articleContentHtml, true);

    showingArticleClip = true;
    showingPhotoClip = false;
    showingVideoClip = false;

    var shortArticleContentHTML = articleContentHtml.cloneNode(true);

    /* Generate short form article markup */
    shortArticleContentHTML = generateShortFormArticleMarkup(shortArticleContentHTML);
    var shortArticleDiv = document.createElement("div");
    shortArticleDiv.appendChild(shortArticleContentHTML);

    /* Dynamically create a form to be POSTed to the iframe */
    var formHtml = '<form id="sl_articleform" style="display: none;" target="sl_articleformiframe" method="post" action="' + sl_articleFormDomain + '">\
		                <input type="hidden" name="articleMarkup" id="bodyContentMarkup" value="' + oceanus.htmlspecialchars(articleDiv.innerHTML) + '" />\
		                <input type="hidden" name="shortArticleMarkup" id="bodyContentMarkup" value="' + oceanus.htmlspecialchars(shortArticleDiv.innerHTML) + '" />\
		                <input type="hidden" name="articleText" id="bodyContentText" value="' + oceanus.htmlspecialchars(articleText) + '" />\
		                <input type="hidden" name="articleTitle" id="bodyContentTitle" value="' + oceanus.htmlspecialchars(articleTitle) + '" />\
						<input type="hidden" name="pageUrl" id="pageUrl" value="' + oceanus.htmlspecialchars(window.location.href) + '" />\
						<input type="hidden" name="pageTitle" id="pageTitle" value="' + oceanus.htmlspecialchars(document.title) + '" />\
						<input type="hidden" name="pageDescription" id="pageDescription" value="' + oceanus.htmlspecialchars(sl_getPageDescription()) + '" />\
                    </form>';

    sl_reloadDiscussion = true;
    document.body.innerHTML += formHtml;
    document.forms['sl_articleform'].submit();

    document.getElementById("sl_iframecontent").style.display = "none";
    if (document.getElementById("sl_photoselect") != null) {
        document.getElementById("sl_photoselect").style.display = "none";
    }
    if (document.getElementById("sl_articleselect") != null) {
        document.getElementById("sl_articleselect").style.display = "none";
    }
    if (document.getElementById("sl_videoselect") != null) {
        document.getElementById("sl_videoselect").style.display = "none";
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }
    sl_clipArticleContainer.style.display = "block";
}

function generateShortFormArticleMarkup(articleMarkup) {
    var imageCount = 0;
    sl_jQuery('img', articleMarkup).each(function () {
        ++imageCount;
        if (imageCount > 1) {
            sl_jQuery(this).remove();
        }
        else {
            sl_jQuery(this).css('min-height', 'auto');
            sl_jQuery(this).css('min-width', 'auto');
        }
    });

    sl_jQuery('table', this).each(function () {
        sl_jQuery(this).remove();
    });

    var articleElementText = oceanus.getInnerText(articleMarkup, false);
    var targetElementTextLength = articleElementText.length * 0.5;

    if (targetElementTextLength > 600) {
        targetElementTextLength = 600;
    }

    var currArticleMarkup = null;
    while (articleElementText.length > targetElementTextLength && currArticleMarkup != articleMarkup.innerHTML) {
        currArticleMarkup = articleMarkup.innerHTML;
        trimArticleContents(articleMarkup, this, null);
        articleElementText = oceanus.getInnerText(articleMarkup, false);
    }

    var horizontalRuleRegEx = /(<hr[^>]*>[ \n\r\t]*){1,}/gi;
    articleMarkup.innerHTML = articleMarkup.innerHTML.replace(horizontalRuleRegEx, '');

    return articleMarkup;
}

function trimArticleContents(root, origRoot, targetHeight) {
    //trim content from article
    var lastParagraph = null;
    var paragraph = sl_jQuery('p, span, div, h3, h4, h5, h6', root);
    if (paragraph.length <= 0) {
        lastParagraph = root;
    }
    else {
        lastParagraph = paragraph[paragraph.length - 1];

        if (sl_jQuery.trim(lastParagraph.innerHTML) == "") {
            lastParagraph.parentNode.removeChild(lastParagraph);
            trimArticleContents(root, origRoot, targetHeight);
            return;
        }
    }

    for (var i = lastParagraph.childNodes.length - 1; i >= 0; i--) {
        var childNode = lastParagraph.childNodes[i];
        if (childNode.nodeType == 3) { //text node
            if (targetHeight == 'undefined' || targetHeight == null || targetHeight <= 0) {
                var text = childNode.nodeValue;
                if (sl_jQuery.trim(text) == "" || sl_jQuery.trim(text) == "...") {
                    childNode.parentNode.removeChild(childNode);
                }
                else {
                    text = text.replace(/\s*\S+\s*$/, "...");
                    childNode.nodeValue = text;
                    break;
                }
            }
            else {
                while (targetHeight > 0 && sl_jQuery(origRoot).height() > targetHeight) {
                    var text = childNode.nodeValue;
                    if (sl_jQuery.trim(text) == "" || sl_jQuery.trim(text) == "...") {
                        childNode.parentNode.removeChild(childNode);
                        break;
                    }
                    else {
                        text = text.replace(/\s*\S+\s*$/, "...");
                        childNode.nodeValue = text;
                    }
                }

                if (sl_jQuery(origRoot).height() < targetHeight) {
                    break;
                }
            }
        }
        else {
            if (childNode.innerHTML == "") {
                childNode.parentNode.removeChild(childNode);
            }
            else {
                trimArticleContents(childNode, origRoot, targetHeight);
                break;
            }
        }
    }

    if (sl_jQuery.trim(lastParagraph.innerHTML) == "") {
        lastParagraph.parentNode.removeChild(lastParagraph);
    }
}

function sl_showShareLinkForm() {
    var description = sl_getPageDescription();

    if ((sl_sharelinkContainer = document.getElementById('sl_sharelinkcontent')) == null) {
        sl_sharelinkContainer = document.createElement("iframe");
        sl_sharelinkContainer.setAttribute("id", "sl_sharelinkcontent");
        sl_sharelinkContainer.setAttribute("allowtransparency", "true");
        sl_sharelinkContainer.setAttribute("frameborder", "0");
        sl_sharelinkContainer.setAttribute("style", "width:962px;clear:both;border:none;display:block;padding: 0 !important;margin:0 auto !important;");
        sl_sharelinkContainer.setAttribute("src", sl_domain + "/webpage/GetShareLinkUI?pageUrl=" + encodeURIComponent(window.location.href) + "&pageTitle=" + encodeURIComponent(document.title) + "&pageDescription=" + encodeURIComponent(description));
        document.getElementById('sl_container').appendChild(sl_sharelinkContainer);
    }

    sl_sharelinkContainer.style.display = "block";
    document.getElementById("sl_iframecontent").style.display = "none";
    if (sl_jQuery('#sl_articleformiframe') != null) {
        sl_jQuery('#sl_articleformiframe').remove();
    }
    if (document.getElementById("sl_photoselect") != null) {
        document.getElementById("sl_photoselect").style.display = "none";
    }
    if (document.getElementById("sl_articleselect") != null) {
        document.getElementById("sl_articleselect").style.display = "none";
    }
    if (document.getElementById("sl_videoselect") != null) {
        document.getElementById("sl_videoselect").style.display = "none";
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }
}

function sl_showVideoClipAddForm(imgSrc, width, height, videoId) {
    if (sl_jQuery('#sl_videoselect') != null) {
        sl_jQuery('#sl_videoselect').hide();
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }
    if (document.forms['sl_loadvideoclipform'] != 'undefined' && document.forms['sl_loadvideoclipform'] != null) {
        document.body.removeChild(document.forms['sl_loadvideoclipform']);
    }

    var sl_clipPhotoFormContainer = document.createElement("iframe");
    sl_clipPhotoFormContainer.setAttribute("id", "sl_addphotoclipform");
    sl_clipPhotoFormContainer.setAttribute("allowtransparency", "true");
    sl_clipPhotoFormContainer.setAttribute("frameborder", "0");
    sl_clipPhotoFormContainer.setAttribute("style", "width:962px;clear:both;border:none;display:block;padding: 0 !important;margin:0 auto !important;");
    document.getElementById('sl_container').appendChild(sl_clipPhotoFormContainer);

    var formHtml = '<form id="sl_loadvideoclipform" style="display: none;" target="sl_addphotoclipform" method="post" action="' + sl_videoClipDomain + '">\
						<input type="hidden" name="pageUrl" id="pageUrl" value="' + oceanus.htmlspecialchars(window.location.href) + '" />\
						<input type="hidden" name="pageTitle" id="pageTitle" value="' + oceanus.htmlspecialchars(document.title) + '" />\
						<input type="hidden" name="pageDescription" id="pageDescription" value="' + oceanus.htmlspecialchars(sl_getPageDescription()) + '" />\
						<input type="hidden" name="imageUrl" id="imageUrl" value="' + oceanus.htmlspecialchars(imgSrc) + '" />\
						<input type="hidden" name="width" id="width" value="' + oceanus.htmlspecialchars(width) + '" />\
						<input type="hidden" name="height" id="width" value="' + oceanus.htmlspecialchars(height) + '" />\
						<input type="hidden" name="videoId" id="width" value="' + oceanus.htmlspecialchars(videoId) + '" />\
                    </form>';

    sl_reloadDiscussion = true;
    document.body.innerHTML += formHtml;
    document.forms['sl_loadvideoclipform'].submit();

    sl_jQuery('#sl_addphotoclipform').show();
}

function sl_showPhotoClipAddForm(imgSrc, width, height) {
    if (sl_jQuery('#sl_photoselect') != null) {
        sl_jQuery('#sl_photoselect').hide();
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }
    if (document.forms['sl_loadphotoclipform'] != 'undefined' && document.forms['sl_loadphotoclipform'] != null) {
        document.body.removeChild(document.forms['sl_loadphotoclipform']);
    }

    var sl_clipPhotoFormContainer = document.createElement("iframe");
    sl_clipPhotoFormContainer.setAttribute("id", "sl_addphotoclipform");
    sl_clipPhotoFormContainer.setAttribute("name", "sl_addphotoclipform");
    sl_clipPhotoFormContainer.setAttribute("allowtransparency", "true");
    sl_clipPhotoFormContainer.setAttribute("frameborder", "0");
    sl_clipPhotoFormContainer.setAttribute("style", "width:962px;clear:both;border:none;display:block;padding: 0 !important;margin:0 auto !important;");
    document.getElementById('sl_container').appendChild(sl_clipPhotoFormContainer);

    var formHtml = '<form id="sl_loadphotoclipform" style="display: none;" target="sl_addphotoclipform" method="post" action="' + sl_photoClipDomain + '">\
						<input type="hidden" name="pageUrl" id="pageUrl" value="' + oceanus.htmlspecialchars(window.location.href) + '" />\
						<input type="hidden" name="pageTitle" id="pageTitle" value="' + oceanus.htmlspecialchars(document.title) + '" />\
						<input type="hidden" name="pageDescription" id="pageDescription" value="' + oceanus.htmlspecialchars(sl_getPageDescription()) + '" />\
						<input type="hidden" name="imageUrl" id="imageUrl" value="' + oceanus.htmlspecialchars(imgSrc) + '" />\
						<input type="hidden" name="width" id="width" value="' + oceanus.htmlspecialchars(width) + '" />\
						<input type="hidden" name="height" id="width" value="' + oceanus.htmlspecialchars(height) + '" />\
                    </form>';

    sl_reloadDiscussion = true;
    document.body.innerHTML += formHtml;
    document.forms['sl_loadphotoclipform'].submit();

    sl_jQuery('#sl_addphotoclipform').show();
}

function sl_showArticleClipUI() {
    if (document.articles == 'undefined' || document.articles == null || document.articles.length < 1) {
        window.alert("Sorry, we can't find articles on this page.");
        return;
    }

    var container = document.getElementById('sl_articleselect');

    if (container == null) {

        container = document.createElement("div");
        container.setAttribute("id", "sl_articleselect");
        container.setAttribute("style", "z-index:2147483647;");
        document.getElementById('sl_container').appendChild(container);

        var title = document.createElement("div");
        title.setAttribute("id", "sl_articleselecttitle");
        container.appendChild(title);
        title.innerHTML = "Select Article To Clip";

        var alignmiddlecont = document.createElement("div");
        alignmiddlecont.setAttribute("id", "sl_articleselect_middle");
        container.appendChild(alignmiddlecont);

        if (document.articles != 'undefined' && document.articles != null) {
            for (var index = 0; index < document.articles.length; ++index) {
                var articleClipLink = document.createElement("a");
                articleClipLink.setAttribute("href", "#");
                articleClipLink.setAttribute("onClick", "javascript:sl_showArticleClipForm(" + index + ");return false;");
                articleClipLink.setAttribute("class", "oceanus-content-link");
                articleClipLink.setAttribute("style", "color:#fff;font-weight:bold;text-decoration:none;font-size:13px;text-shadow: 0 -1px rgba(34, 25, 25, .3);");
                articleClipLink.innerHTML = "Clip Article";

                var articleElem = document.articles[index].cloneNode(true);
                sl_jQuery('a', articleElem).each(function () {
                    var newNode = document.createElement('span');
                    try {
                        newNode.innerHTML = this.innerHTML;
                        this.parentNode.replaceChild(newNode, this);
                    }
                    catch (e) {
                    }
                });

                articleElem.insertBefore(articleClipLink, articleElem.firstChild);

                alignmiddlecont.appendChild(articleElem);
            }
        }
    }

    document.getElementById("sl_iframecontent").style.display = "none";
    if (sl_clipphotoformContainer != null) {
        sl_clipphotoformContainer.style.display = "none";
    }
    if (sl_sharelinkContainer != null) {
        sl_sharelinkContainer.style.display = "none";
    }
    if (sl_jQuery('#sl_articleformiframe') != null) {
        sl_jQuery('#sl_articleformiframe').remove();
    }
    if (document.getElementById("sl_photoselect") != null) {
        document.getElementById("sl_photoselect").style.display = "none";
    }
    if (document.getElementById("sl_videoselect") != null) {
        document.getElementById("sl_videoselect").style.display = "none";
    }
    if (document.getElementById("sl_articleselect") != null) {
        document.getElementById("sl_articleselect").style.display = "block";
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }
}

function sl_showVideoClipUI(images, videoIds) {

    var container = document.getElementById('sl_videoselect');

    if (container != null) {
        container.parentNode.removeChild(container);
    }

    container = document.createElement("div");
    container.setAttribute("id", "sl_videoselect");
    document.getElementById('sl_container').appendChild(container);

    var title = document.createElement("div");
    title.setAttribute("id", "sl_photoselecttitle");
    container.appendChild(title);
    title.innerHTML = "Select Video To Clip";

    var alignmiddlecont = document.createElement("div");
    alignmiddlecont.setAttribute("id", "sl_photoselect_middle");
    alignmiddlecont.setAttribute("style", "z-index:2147483647;");
    container.appendChild(alignmiddlecont);

    for (var i = 0; i < images.length; ++i) {
        var photocontainer = document.createElement("div");
        photocontainer.setAttribute("class", "sl_photo_select_cont");

        var link = document.createElement("a");
        link.href = "#";
        link.onclick = function (j, k, l, m) { return function () { sl_showVideoClipAddForm(j, k, l, m); return false; }; } (images[i].src, images[i].width, images[i].height, videoIds[i]);

        photocontainer.appendChild(link);

        var img = document.createElement("img");
        img.src = images[i].src;
        img.setAttribute("class", "sl_photo_select_img");
        img.setAttribute("style", "" + sl_getPhotoSelectImgStyle(images[i]));

        link.appendChild(img);

        var imgCaption = document.createElement("span");
        imgCaption.innerHTML = images[i].width + " x " + images[i].height;
        imgCaption.setAttribute("class", "sl_photo_select_img_caption");

        link.appendChild(imgCaption);

        alignmiddlecont.appendChild(photocontainer);
    }

    document.getElementById("sl_iframecontent").style.display = "none";
    if (sl_clipphotoformContainer != null) {
        sl_clipphotoformContainer.style.display = "none";
    }
    if (sl_sharelinkContainer != null) {
        sl_sharelinkContainer.style.display = "none";
    }
    if (sl_jQuery('#sl_articleformiframe') != null) {
        sl_jQuery('#sl_articleformiframe').remove();
    }
    if (document.getElementById("sl_photoselect") != null) {
        document.getElementById("sl_photoselect").style.display = "none";
    }
    if (document.getElementById("sl_articleselect") != null) {
        document.getElementById("sl_articleselect").style.display = "none";
    }
    if (document.getElementById("sl_videoselect") != null) {
        document.getElementById("sl_videoselect").style.display = "block";
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }
}

function sl_showPhotoClipUI(images) {

    var container = document.getElementById('sl_photoselect');

    if (container != null) {
        container.parentNode.removeChild(container);
    }

    container = document.createElement("div");
    container.setAttribute("id", "sl_photoselect");
    document.getElementById('sl_container').appendChild(container);

    var title = document.createElement("div");
    title.setAttribute("id", "sl_photoselecttitle");
    container.appendChild(title);
    title.innerHTML = "Select Photo To Clip";

    var alignmiddlecont = document.createElement("div");
    alignmiddlecont.setAttribute("id", "sl_photoselect_middle");
    alignmiddlecont.setAttribute("style", "z-index:2147483647;");
    container.appendChild(alignmiddlecont);

    for (var i = 0; i < images.length; ++i) {
        var photocontainer = document.createElement("div");
        photocontainer.setAttribute("class", "sl_photo_select_cont");

        var link = document.createElement("a");
        link.href = "#";
        link.onclick = function (j, k, l) { return function () { sl_showPhotoClipAddForm(j, k, l); return false; }; } (images[i].src, images[i].width, images[i].height);

        photocontainer.appendChild(link);

        var img = document.createElement("img");
        img.src = images[i].src;
        img.setAttribute("class", "sl_photo_select_img");
        img.setAttribute("style", "" + sl_getPhotoSelectImgStyle(images[i]));

        link.appendChild(img);

        var imgCaption = document.createElement("span");
        imgCaption.innerHTML = images[i].width + " x " + images[i].height;
        imgCaption.setAttribute("class", "sl_photo_select_img_caption");

        link.appendChild(imgCaption);

        alignmiddlecont.appendChild(photocontainer);
    }

    document.getElementById("sl_iframecontent").style.display = "none";
    if (sl_clipphotoformContainer != null) {
        sl_clipphotoformContainer.style.display = "none";
    }
    if (sl_sharelinkContainer != null) {
        sl_sharelinkContainer.style.display = "none";
    }
    if (sl_jQuery('#sl_articleformiframe') != null) {
        sl_jQuery('#sl_articleformiframe').remove();
    }
    if (document.getElementById("sl_photoselect") != null) {
        document.getElementById("sl_photoselect").style.display = "block";
    }
    if (document.getElementById("sl_articleselect") != null) {
        document.getElementById("sl_articleselect").style.display = "none";
    }
    if (document.getElementById("sl_videoselect") != null) {
        document.getElementById("sl_videoselect").style.display = "none";
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }
}

function sl_showPipflyLogin() {
    if (document.getElementById("sl_photoselect") != null) {
        document.getElementById("sl_photoselect").style.display = "none";
    }
    if (document.getElementById("sl_videoselect") != null) {
        document.getElementById("sl_videoselect").style.display = "none";
    }
    if (document.getElementById("sl_articleselect") != null) {
        document.getElementById("sl_articleselect").style.display = "none";
    }
    if (sl_clipphotoformContainer != null) {
        sl_clipphotoformContainer.style.display = "none";
    }
    if (sl_jQuery('#sl_articleformiframe') != null) {
        sl_jQuery('#sl_articleformiframe').remove();
    }
    if (sl_sharelinkContainer != null) {
        sl_sharelinkContainer.style.display = "none";
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }

    if (document.getElementById('sl_iframecontent') != 'undefined' && document.getElementById('sl_iframecontent') != null) {
        document.getElementById('sl_container').removeChild(document.getElementById('sl_iframecontent'));
    }

    var frame = document.createElement("iframe");
    frame.setAttribute("id", "sl_iframecontent");
    frame.setAttribute("name", "sl_iframecontent");
    frame.setAttribute("allowtransparency", "true");
    frame.setAttribute("frameborder", "0");
    frame.setAttribute("src", sl_LoginDomain + "?pageurl=" + window.location.href);
    frame.setAttribute("style", "width:962px;clear:both;border:none;display:block;padding: 0 !important;margin:0 auto !important;z-index:2147483647;");
    document.getElementById('sl_container').appendChild(frame);
}

function sl_showDiscussion() {
    if (document.getElementById("sl_photoselect") != null) {
        document.getElementById("sl_photoselect").style.display = "none";
    }
    if (document.getElementById("sl_videoselect") != null) {
        document.getElementById("sl_videoselect").style.display = "none";
    }
    if (document.getElementById("sl_articleselect") != null) {
        document.getElementById("sl_articleselect").style.display = "none";
    }
    if (sl_clipphotoformContainer != null) {
        sl_clipphotoformContainer.style.display = "none";
    }
    if (sl_jQuery('#sl_articleformiframe') != null) {
        sl_jQuery('#sl_articleformiframe').remove();
    }
    if (sl_sharelinkContainer != null) {
        sl_sharelinkContainer.style.display = "none";
    }
    if (sl_jQuery('#sl_addphotoclipform') != null) {
        sl_jQuery('#sl_addphotoclipform').remove();
    }

    if (sl_reloadDiscussion) {
        if (document.forms['sl_loadwebpageform'] != 'undefined' && document.forms['sl_loadwebpageform'] != null) {
            document.body.removeChild(document.forms['sl_loadwebpageform']);
        }

        if (document.getElementById('sl_iframecontent') != 'undefined' && document.getElementById('sl_iframecontent') != null) {
            document.getElementById('sl_container').removeChild(document.getElementById('sl_iframecontent'));
        }

        var frame = document.createElement("iframe");
        frame.setAttribute("id", "sl_iframecontent");
        frame.setAttribute("name", "sl_iframecontent");
        frame.setAttribute("allowtransparency", "true");
        frame.setAttribute("frameborder", "0");
        frame.setAttribute("style", "width:962px;clear:both;border:none;display:block;padding: 0 !important;margin:0 auto !important;");
        document.getElementById('sl_container').appendChild(frame);

        var formHtml = '<form id="sl_loadwebpageform" style="display: none;" target="sl_iframecontent" method="post" action="' + sl_dashboardDomain + '">\
						<input type="hidden" name="pageUrl" id="pageUrl" value="' + oceanus.htmlspecialchars(window.location.href) + '" />\
						<input type="hidden" name="pageTitle" id="pageTitle" value="' + oceanus.htmlspecialchars(document.title) + '" />\
						<input type="hidden" name="pageDescription" id="pageDescription" value="' + oceanus.htmlspecialchars(sl_getPageDescription()) + '" />\
                    </form>';

        document.body.innerHTML += formHtml;
        document.forms['sl_loadwebpageform'].submit();

        sl_reloadDiscussion = false;
    }
    else {
        document.getElementById("sl_iframecontent").style.display = "block";
    }
}

function sl_getPhotoSelectImgStylePerHeight(width, height) {
    if (Math.max(height, width) > 233) {
        if (height < width) {
            return "margin-top: " + parseInt(116 - 116 * (height / width)) + "px;";
        }
        return "";
    } else {
        return "margin-top: " + parseInt(116 - height / 2) + "px;";
    }
}

function sl_getPhotoSelectImgStyle(img) {
    if (Math.max(img.height, img.width) > 233) {
        if (img.height < img.width) {
            return "margin-top: " + parseInt(116 - 116 * (img.height / img.width)) + "px;";
        }
        return "";
    } else {
        return "margin-top: " + parseInt(116 - img.height / 2) + "px;";
    }
}

function isHidden(who) {
    var sty;
    if (who.style.display == 'none' || who.style.visibility == 'hidden') return true;
    if (who.currentStyle) {
        sty = who.currentStyle;
        if (sty.display.toLowerCase() == 'none' || sty.visibility.toLowerCase() == 'hidden') return true;
    }
    else if (document.defaultView) {
        sty = document.defaultView.getComputedStyle(who, '');
        if (sty.getPropertyValue('display') == 'none' ||
       sty.getPropertyValue('visibility') == 'hidden') return true
    }
    return false;
}