/*
* Copyright (c) 2012-2014 AssimpNet - Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Assimp.Unmanaged;

namespace Assimp
{
    /// <summary>
    /// Describes a file format which Assimp can export to.
    /// </summary>
    [DebuggerDisplay("{Description}")]
    public sealed class ImportFormatDescription
    {
        private String m_formatId;
        private String m_description;
        private String m_fileExtension;

        private uint m_flags;

        /** Indicates that there is a textual encoding of the
            *  file format; and that it is supported.*/
        private const uint aiImporterFlags_SupportTextFlavour = 0x1;

        /** Indicates that there is a binary encoding of the
            *  file format; and that it is supported.*/
        private const uint aiImporterFlags_SupportBinaryFlavour = 0x2;

        /** Indicates that there is a compressed encoding of the
            *  file format; and that it is supported.*/
        private const uint aiImporterFlags_SupportCompressedFlavour = 0x4;

        /** Indicates that the importer reads only a very particular
            * subset of the file format. This happens commonly for
            * declarative or procedural formats which cannot easily
            * be mapped to #aiScene */
        private const uint aiImporterFlags_LimitedSupport = 0x8;

        /** Indicates that the importer is highly experimental and
            * should be used with care. This only happens for trunk
            * (i.e. SVN) versions, experimental code is not included
            * in releases. */
        private const uint aiImporterFlags_Experimental = 0x10;

        /// <summary>
        /// Gets a short string ID to uniquely identify the export format. E.g. "dae" or "obj".
        /// </summary>
        public String FormatId
        {
            get
            {
                return m_formatId;
            }
        }

        /// <summary>
        /// Gets a short description of the file format to present to users.
        /// </summary>
        public String Description
        {
            get
            {
                return m_description;
            }
        }

        /// <summary>
        /// Gets the recommended file extension for the exported file in lower case.
        /// </summary>
        public String FileExtension
        {
            get
            {
                return m_fileExtension;
            }
        }

        public bool SupportsTextFlavour
        {
            get
            {
                return ((m_flags & aiImporterFlags_SupportTextFlavour) > 0) ? true : false;
            }
        }

        public bool SuportBinaryFlavour
        {
            get
            {
                return ((m_flags & aiImporterFlags_SupportBinaryFlavour) > 0) ? true : false;
            }
        }

        public bool SuportCompressedFlavour
        {
            get
            {
                return ((m_flags & aiImporterFlags_SupportCompressedFlavour) > 0) ? true : false;
            }
        }

        public bool LimitedSupport
        {
            get
            {
                return ((m_flags & aiImporterFlags_LimitedSupport) > 0) ? true : false;
            }
        }

        public bool Experimental
        {
            get
            {
                return ((m_flags & aiImporterFlags_Experimental) > 0) ? true : false;
            }
        }

        /// <summary>
        /// Constructs a new ExportFormatDescription.
        /// </summary>
        /// <param name="formatDesc">Unmanaged structure</param>
        internal ImportFormatDescription(ref AiImporterDesc formatDesc, int id)
        {
            m_formatId = id.ToString();
            m_description = Marshal.PtrToStringAnsi(formatDesc.mName);
            m_fileExtension = Marshal.PtrToStringAnsi(formatDesc.mFileExtensions);
            m_flags = formatDesc.mFlags;
            //Stupid hack, for some reason the formatID for COLLADA format is always messed up
            if (m_fileExtension == "dae")
                m_formatId = "collada";
        }
    }
}
